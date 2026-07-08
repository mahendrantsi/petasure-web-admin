namespace Project.WebAPI
{
    using System;
    using System.Text;
    using AutoMapper;
    //using FluentValidation;
    // using FluentValidation.AspNetCore;
    using Project.Core.ExceptionHandler;
    using Project.Data;
    using Project.Data.ExtendedDBEntities;
    using Project.Logger;
    using Project.Middleware.Extensions;
    using Project.Models.AccountModel;
    using Project.Models.CommonModel;
    using Project.Persistence.UOW;
    using Project.Services.IService;
    using Project.Services.Mapping;
    using Project.Services.Service;
    using Project.Services.ServiceEntities;
    using Project.WebAPI.Auth;
    using Project.WebAPI.Helpers;
    using Project.WebAPI.Infrastructure;
    using Project.WebAPI.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Project.Data.DBEntities;
    using System.Net;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Project.Services.ServiceHelper;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; //  todo: get this from somewhere secure
        private readonly SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        private const string ApiSourceHeader = "X-ApiSource";
        private string _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env.EnvironmentName;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ProjectDbConnection"));
            });

            services.AddMvc().AddDataAnnotationsLocalization();
            services.AddAutoMapper(typeof(ViewModelToDatabaseMappingProfile).Assembly);
            services.AddIdentity<DerivedIdentityUser, IdentityRole<Guid>>().AddEntityFrameworkStores<ProjectDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ILoggerManager, LoggerManager>();
            services.AddTransient<IAccountService, AccountService>();
            services.Configure<EmailConfig>(this.Configuration.GetSection("Email"));
            
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailLogService, EmailLogService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IIntegrationService, IntegrationService>();
            services.AddTransient<IHistoryService, HistoryService>();
            services.AddTransient<IIntegrationService, IntegrationService>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<IExceptionLoggerService, ExceptionLoggerService>();
            services.AddTransient<ISubscriptionService, SubscriptionService>();
           
            //Created By Manthan tak
            services.AddTransient<IContentService, ContentService>();
            //Created By Manthan Tak
            services.AddTransient<IPetService, PetService>();
            services.AddTransient<IMissingService, MissingService>();

            services.AddTransient<ISystemSetting, SystemSetting>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", //  must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
            });


            var jwtTokenConfig = Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),

                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CheckUser", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    //policy.Requirements.Add(new EditUserRequirement());
                });
            });

            services.AddTransient<IJwtAuthManager, JwtAuthManager>();
            services.AddHostedService<JwtRefreshTokenCache>();
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
                app.UseExceptionHandler("/Error");
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {

                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                    var db = serviceScope.ServiceProvider.GetRequiredService<ProjectDbContext>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Database migration failed. Ensure the database exists and the user has sufficient permissions.");
                }

               // var context = serviceScope.ServiceProvider.GetRequiredService<ProjectDbContext>();
               // context.Database.Migrate();

                //var setting = serviceScope.ServiceProvider.GetRequiredService<ISettingService>();
                //var response = setting.GetSettings().Result;
                //if (response.IsSuccess)
                //    (serviceScope.ServiceProvider.GetRequiredService<ISystemSetting>()).SetSystemVariables(response.Data);

            }

            app.UseMiddleware(x =>
            {
                x.GenericMessage = "An error has occurred.  Please try again in a few minutes.  If the problem persists, please contact Customer Support";
                x.ResponseFormatExclude = new[] { "/index.html", "/swagger" };
            });
            app.Use((context, next) =>
            {
                context.Response.Headers.Add(ApiSourceHeader, "Project");
                return next.Invoke();
            });
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                c.DocumentTitle = "Project";
                c.RoutePrefix = string.Empty;
            });


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
