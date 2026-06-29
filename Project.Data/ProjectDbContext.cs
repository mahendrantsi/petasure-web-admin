using Project.Data;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.Configuration;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection.Emit;
using Project.Core.Extension;
using Project.Data.DbModels;

namespace Project
{
    public class ProjectDbContext : IdentityDbContext<DerivedIdentityUser, IdentityRole<Guid>, Guid>
    {
        private readonly IConfiguration configuration;
        public ProjectDbContext() { }

        public ProjectDbContext(DbContextOptions options, IConfiguration iConfig) : base(options)
        {
            configuration = iConfig;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(configuration.GetSection("ConnectionStrings").GetSection("ProjectDbConnection").Value);
                //optionsBuilder.UseSqlServer("Data Source=RAHULPATEL\\MSSQLSERVERDEV17;Initial Catalog=projDB;Persist Security Info=True;User ID=ProUser;Password=Dots@1234;TrustServerCertificate=True;");
                //optionsBuilder.UseSqlServer("Data Source=MANTHAN\\SQLEXPRESS;Initial Catalog=petasure;Persist Security Info=True;User ID=petasure;Password=Dots@1234;TrustServerCertificate=True;");
                //optionsBuilder.UseSqlServer("data source=subhashpc\\sqlexpress;initial catalog=petasure;integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=30;TrustServerCertificate=True;");
                optionsBuilder.UseSqlServer("Server=TSI0287-VIGNESH\\SQLEXPRESS;Database=Petasure;Integrated Security=True;TrustServerCertificate=True;");

            }
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<DerivedIdentityRole> DerivedIdentityRole { get; set; }
        public DbSet<DerivedIdentityUser> DerivedIdentityUser { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<EmailLog> EmailLog { get; set; }
        public DbSet<ExceptionLogger> ExceptionLogger { get; set; }
        public DbSet<Integration> Integration { get; set; }
        public DbSet<ContentInfo> ContentInfo { get; set; }
        public DbSet<tblCountry> tblCountry { get; set; } 
        public DbSet<UserPasswordToken> UserPasswordToken { get; set; }
        public DbSet<FAQ> FAQ { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public DbSet<PetInfo> PetInfo { get; set; }
        public DbSet<PetTypeMaster> PetTypeMaster { get; set; }

        public DbSet<MissingPets> MissingPet { get; set; }

        public DbSet<MissingPetsLogs> MissingPetLogs { get; set; }
        public DbSet<Enquiry> Enquiry { get; set; }
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<InAppPurchases> InAppPurchases { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var dbModle = new DbModel();
            dbModle.UserOnModelCreating(builder);
        }
    }
}
