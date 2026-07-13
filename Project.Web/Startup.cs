using Project.Data;
using Project.Logger;
using Project.Models.CommonModel;
using Project.Persistence.UOW;
using Project.Services.IService;
using Project.Services.Mapping;
using Project.Services.Service;
using Project.Services.ServiceEntities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NToastNotify;
using System;
using Project.Data.DBEntities;
using AspNetCore.ServiceRegistration.Dynamic;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json.Serialization;
using Project.Data.ExtendedDBEntities;
using Project.Web.WebResource.Middleware;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using ServiceStack;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Project.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ProjectDbConnection"));
            });

            //services.AddMvc().AddFluentValidation(fv =>
            //{
            //    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            //    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            //});

            services.AddMvc().AddDataAnnotationsLocalization().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopRight
            });
            services.AddAutoMapper(typeof(ViewModelToDatabaseMappingProfile).Assembly);
            services.AddIdentity<DerivedIdentityUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ProjectDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAccountService, AccountService>();
            services.Configure<EmailConfig>(this.Configuration.GetSection("Email"));            

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailLogService, EmailLogService>();
            services.AddTransient<IContentService, ContentService>();
            services.AddTransient<IPetService, PetService>();
            services.AddTransient<IMissingService, MissingService>();



            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            }).AddRazorRuntimeCompilation()
              .AddApplicationPart(typeof(NToastNotify.ToastrOptions).Assembly);

            services.AddControllers().AddNewtonsoftJson();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IHistoryService, HistoryService>();
            services.AddTransient<IExceptionLoggerService, ExceptionLoggerService>();
            services.AddTransient<IEmailService, EmailService>(); 
            services.AddTransient<IIntegrationService, IntegrationService>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<ISystemSetting, SystemSetting>();
            services.AddTransient<IExceptionLoggerService, ExceptionLoggerService>();
            services.AddTransient<ISubscriptionService, SubscriptionService>();
            services.AddTransient<IAlertCentreService, AlertCentreService>();


            services.AddRazorPages();
            //services.AddMicrosoftIdentityWebAppAuthentication(Configuration);
            //services.AddMvc(option =>
            //{
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //    option.Filters.Add(new AuthorizeFilter(policy));
            //}).AddMicrosoftIdentityUI();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = AzureADDefaults.OpenIdScheme;
            //})
            ////.AddAzureAD(options => Configuration.Bind("AzureAd", options))
            //.AddCookie(); 

            //services.AddAuthorization(options =>
            //{
            //    // Add authorization policies as needed.
            //});



            // Host.CreateDefaultBuilder()
            //.ConfigureServices(services =>
            //{
            //    services.AddHostedService<MyScheduler>();
            //}).Build().Run();

            // host.RunAsync().ConfigureAwait(false);


            //services.AddQuartz(q =>
            //{
            //    q.UseMicrosoftDependencyInjectionJobFactory();


            //    // q.UseMicrosoftDependencyInjectionScopedJobFactory();
            //    var jobKey = new JobKey("ScheduledTransactionStatusJob");
            //    var jobKey2 = new JobKey("ScheduledPaymentStatusJob");
            //    q.AddJob<Jobs.ScheduledTransactionStatusJob>(opts => opts.WithIdentity(jobKey));
            //    q.AddJob<Jobs.ScheduledPaymentStatusJob>(opts => opts.WithIdentity(jobKey2));

            //    q.AddTrigger(opts =>
            //    {
            //        opts.ForJob(jobKey).WithIdentity("DemoJob-trigger").WithSimpleSchedule(x => x.RepeatForever().WithIntervalInMinutes(30));
            //        //.WithCronSchedule("0 0/30 * * * ?");

            //        //   opts.ForJob(jobKey2).WithIdentity("DemoJob-trigger").WithCronSchedule("0 0/2 * * * ?");
            //    });

            //    q.AddTrigger(opts => opts.ForJob(jobKey2).WithIdentity("DemoJob-trigger2").WithSimpleSchedule(x => x.RepeatForever().WithIntervalInMinutes(2))); // .WithCronSchedule("0 0/2 * * * ?"));
            //    //.WithCronSchedule("0 */30 * * * *"));
            //});


            ////services.AddQuartz(q =>
            ////{
            ////    q.UseMicrosoftDependencyInjectionJobFactory();
            ////    var jobKey = new JobKey("ScheduledPaymentStatusJob");
            ////    q.AddJob<Jobs.ScheduledPaymentStatusJob>(opts => opts.WithIdentity(jobKey)); 
            ////    q.AddTrigger(opts => opts.ForJob(jobKey).WithIdentity("DemoJob-trigger").WithCronSchedule("0 0/2 * * * ?")); 
            ////});

           // services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var pathBase = Configuration["CustomKeys:PathBase"]; if (!string.IsNullOrWhiteSpace(pathBase))
            {
                app.UsePathBase(pathBase);
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseFileServer();
            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    //var context = serviceScope.ServiceProvider.GetRequiredService<ProjectDbContext>();
            //    //context.Database.Migrate();
            //    var setting = serviceScope.ServiceProvider.GetRequiredService<ISettingService>();
            //    var response = setting.GetSettings().Result;
            //    if (response.IsSuccess)
            //        (serviceScope.ServiceProvider.GetRequiredService<ISystemSetting>()).SetSystemVariables(response.Data);
            //}

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/json" // or "application/pkcs7-mime" depending on your needs
            });

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            //try
            //{
            //    app.UseStaticFiles(new StaticFileOptions
            //    {
            //        FileProvider = new PhysicalFileProvider(
            //          Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
            //        RequestPath = "/.well-known/apple-app-site-association",
            //       // ServeUnknownFileTypes = true,
            //    });
            //}
            //catch (Exception ex)
            //{

            //    using (var db = new ProjectDbContext())
            //    {
            //        db.Add<ExceptionLogger>(new ExceptionLogger() { InnerException = ex.InnerException?.ToString(), Exception = ex.ToString() });
            //        db.SaveChanges();
            //    }
            //}

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           
            if (!env.IsDevelopment())
            {
                app.UseMiddleware<ErrorHandlingMiddleware>();
            }
            app.UseMiddleware<RolepermissionMiddleware>();


            app.UseNToastNotify();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
            name: "Area",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");

            });
            app.UseCookiePolicy();
        }
    }
}
