using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DbModels
{
    public partial class DbModel
    {
        public void UserOnModelCreating(ModelBuilder builder)
        {
            var userGuid = Guid.Parse("4B79E105-758C-4FBC-9333-4BE0B74BC3F8");
            var roleGuid1 = Guid.Parse("F1213165-FE5F-4750-AFFC-1B3136FD613B");
            var roleGuid2 = Guid.Parse("0B9F1B81-5C09-4237-BCC9-0390044EBF0D");
            var roleGuid3 = Guid.Parse("D5C13504-9424-4E06-ABE9-A74CCBB5C056");
            var SecondayUser = Guid.Parse("6FF06E0D-3E8D-4F9E-BBE9-7EF907BFF3A8");
            var AnonymousUser = Guid.Parse("1F729636-EBDD-42A1-8633-A43DE9A5668B");

            builder.Entity<DerivedIdentityUser>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
            });
            builder.Entity<IdentityUserRole<Guid>>().HasKey(iur => new { iur.UserId, iur.RoleId });


            builder.Entity<DerivedIdentityRole>().HasData(
                new DerivedIdentityRole { Id = roleGuid1, NormalizedName = "ADMIN", Name = "Admin", CreatedOn = DateTime.Parse("2024-11-10", CultureInfo.InvariantCulture), IsActive = true, },
                new DerivedIdentityRole { Id = roleGuid2, NormalizedName = "USER", Name = "User", CreatedOn = DateTime.Parse("2024-11-10", CultureInfo.InvariantCulture), IsActive = true },
                new DerivedIdentityRole { Id = roleGuid3, NormalizedName = "SUBUSER", Name = "SubUser", CreatedOn = DateTime.Parse("2024-11-10", CultureInfo.InvariantCulture), IsActive = true },
                new DerivedIdentityRole { Id = SecondayUser, NormalizedName = "SECONDAYUSER", Name = "SecondayUser", CreatedOn = DateTime.Parse("2024-11-10", CultureInfo.InvariantCulture), IsActive = true },
                new DerivedIdentityRole { Id = AnonymousUser, NormalizedName = "ANONYMOUSUSER", Name = "AnonymousUser", CreatedOn = DateTime.Parse("2024-11-10", CultureInfo.InvariantCulture), IsActive = true }
            );
            builder.Entity<DerivedIdentityUser>().HasData(
                new DerivedIdentityUser
                {
                    Id = userGuid,
                    FirstName = "Dotsquare",
                    LastName = "Admin",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedOn = DateTime.Parse("2024-11-10", CultureInfo.InvariantCulture),
                    AccessFailedCount = 0,
                    Email = "dsadmin@yopmail.com",
                    NormalizedEmail = "DSADMIN@YOPMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "APz2fRvKE8u+ZBkGL+e2crbWGxSPiIPW/QqUnZiPGizQcA5FNToy/ED5JYV7+ujpiQ==" /*here password is Admin@123*/,
                    SecurityStamp = "BRYBDSPPOB5WW7REAMP2I55HBJGGO3VU",
                    ConcurrencyStamp = "9ca8abe8-a776-4f8b-9a6e-795ed3407f1a",
                    PhoneNumber = "7037353635",
                    PhoneNumberConfirmed = true,
                    PhoneNumberConfirmedOn = null,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    LockoutEnd = null,
                    IsDeviceConnected = false,
                    RefreshToken = null,
                    RefreshTokenExpiryTime = DateTime.Parse("2030-11-10", CultureInfo.InvariantCulture),

                }
            );


            var settingID = Guid.Parse("B7525CB0-0EC3-4146-A0FB-E80C7902908A");
            builder.Entity<Settings>().HasData(
               new Settings
               {
                   Id = settingID,
                   IsEmailConfirmed = true,
                   IsPhoneVerificationRequired = false,
                   CreatedBy = userGuid,
                   CreatedOn = DateTime.Parse("2024-11-10", CultureInfo.InvariantCulture)
               }
           );

            //builder.Entity<DerivedIdentityUser>().HasQueryFilter(x => x.IsActive == true && x.IsDeleted == false);
            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> { RoleId = roleGuid1, UserId = userGuid });

            builder.Entity<PetInfo>().HasQueryFilter(x => x.IsDelete == false);

            // ============================================================
            // ===== FOREIGN KEY RELATIONSHIPS & INDEXES CONFIGURATION =====
            // ============================================================

            // 1️⃣ UserProfile.UserId → AspNetUsers(Id) [Set Null]
            builder.Entity<UserProfile>(entity =>
            {
                entity.HasIndex(e => e.UserId).HasDatabaseName("IX_UserProfile_UserId");

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_UserProfile_AspNetUsers_UserId");
            });

            // 2️⃣ InAppPurchases.AspnetuserId → AspNetUsers(Id) [Set Null]
            builder.Entity<InAppPurchases>(entity =>
            {
                entity.HasIndex(e => e.AspnetuserId).HasDatabaseName("IX_InAppPurchases_AspnetuserId");

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.AspnetuserId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_InAppPurchases_AspNetUsers_AspnetuserId");
            });

            // 3️⃣ UserPasswordToken.UserID → AspNetUsers(Id) [Set Null]
            builder.Entity<UserPasswordToken>(entity =>
            {
                entity.HasIndex(e => e.UserID).HasDatabaseName("IX_UserPasswordToken_UserID");

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserID)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_UserPasswordToken_AspNetUsers_UserID");
            });

            // 4️⃣ PetInfo.UserID → AspNetUsers(Id) [Set Null]
            builder.Entity<PetInfo>(entity =>
            {
                entity.HasIndex(e => e.UserID).HasDatabaseName("IX_PetInfo_UserID");

                entity.HasOne(e => e.Owner)
                      .WithMany()
                      .HasForeignKey(e => e.UserID)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_PetInfo_AspNetUsers_UserID");
            });

            // 5️⃣ MissingPets.PetId → PetInfo(Id) [Set Null]
            builder.Entity<MissingPets>(entity =>
            {
                entity.HasIndex(e => e.PetId).HasDatabaseName("IX_MissingPets_PetId");

                entity.HasOne(e => e.Pet)
                      .WithMany(p => p.MissingPets)
                      .HasForeignKey(e => e.PetId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_MissingPets_PetInfo_PetId");
            });

            // 6️⃣ MissingPets.FoundBy → AspNetUsers(Id) [Set Null]
            builder.Entity<MissingPets>(entity =>
            {
                entity.HasIndex(e => e.FoundBy).HasDatabaseName("IX_MissingPets_FoundBy");

                entity.HasOne(e => e.FoundByUser)
                      .WithMany()
                      .HasForeignKey(e => e.FoundBy)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_MissingPets_AspNetUsers_FoundBy");
            });

            // 7️⃣ MissingPetsLogs.MissingPetsID → MissingPets(Id) [Set Null]
            builder.Entity<MissingPetsLogs>(entity =>
            {
                entity.HasIndex(e => e.MissingPetsID).HasDatabaseName("IX_MissingPetsLogs_MissingPetsID");

                entity.HasOne(e => e.MissingPet)
                      .WithMany(m => m.Logs)        
                      .HasForeignKey(e => e.MissingPetsID)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_MissingPetsLogs_MissingPets_MissingPetsID");
            });

            // 8️⃣ MissingPetsLogs.PetId → PetInfo(Id) [Set Null]
            builder.Entity<MissingPetsLogs>(entity =>
            {
                entity.HasIndex(e => e.PetId).HasDatabaseName("IX_MissingPetsLogs_PetId");

                entity.HasOne(e => e.Pet)
                      .WithMany(p => p.MissingPetsLogs)
                      .HasForeignKey(e => e.PetId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_MissingPetsLogs_PetInfo_PetId");
            });

            // 9️⃣ Enquiry.UserId → AspNetUsers(Id) [Set Null] - Optional
            builder.Entity<Enquiry>(entity =>
            {
                entity.HasIndex(e => e.UserId).HasDatabaseName("IX_Enquiry_UserId");

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_Enquiry_AspNetUsers_UserId");
            });

            // ============================================================
            // ===== HEALTH-CHECK / EARLY ISSUE DETECTION SCHEMA ==========
            // ============================================================

            // 🔟 health_check_events.PetId → PetInfo(Id) [Set Null]
            //    Nullable FK + SET NULL keeps stored images/results for the dataset even
            //    if the pet profile is later deleted (consistent with the PetInfo FK style).
            builder.Entity<HealthCheckEvent>(entity =>
            {
                entity.ToTable("health_check_events");

                entity.HasIndex(e => e.PetId).HasDatabaseName("IX_health_check_events_PetId");

                entity.HasOne(e => e.Pet)
                      .WithMany()
                      .HasForeignKey(e => e.PetId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_health_check_events_PetInfo_PetId");
            });

            // 1️⃣1️⃣ health_status.HealthCheckEventId → health_check_events(Id) [Cascade]
            //    A finding has no meaning without its parent event, so it cascade-deletes.
            builder.Entity<HealthStatus>(entity =>
            {
                entity.ToTable("health_status");

                entity.HasIndex(e => e.HealthCheckEventId).HasDatabaseName("IX_health_status_HealthCheckEventId");

                entity.HasOne(e => e.HealthCheckEvent)
                      .WithMany(h => h.HealthStatuses)
                      .HasForeignKey(e => e.HealthCheckEventId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_health_status_health_check_events_HealthCheckEventId");
            });

            // ============================================================
            // ===== PET RECOGNITION SCHEMA ================================
            // ============================================================

            // pet_images.PetId → PetInfo(Id) [Set Null]
            //    Nullable FK: unresolved until a match/registration completes.
            builder.Entity<PetImages>(entity =>
            {
                entity.ToTable("pet_images");

                entity.HasIndex(e => e.PetId).HasDatabaseName("IX_pet_images_PetId");

                entity.HasOne(e => e.Pet)
                      .WithMany()
                      .HasForeignKey(e => e.PetId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_pet_images_PetInfo_PetId");
            });

            // pet_scans.PetId → PetInfo(Id) [Set Null]
            // pet_scans.PrimaryImageId / SecondaryImageId → pet_images(Id) [Restrict]
            //    Restrict (not Cascade/SetNull): deleting an image shouldn't silently
            //    delete scan history.
            builder.Entity<PetScans>(entity =>
            {
                entity.ToTable("pet_scans");

                entity.HasIndex(e => e.PetId).HasDatabaseName("IX_pet_scans_PetId");

                entity.HasOne(e => e.Pet)
                      .WithMany()
                      .HasForeignKey(e => e.PetId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_pet_scans_PetInfo_PetId");

                entity.HasOne(e => e.PrimaryImage)
                      .WithMany()
                      .HasForeignKey(e => e.PrimaryImageId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_pet_scans_pet_images_PrimaryImageId");

                entity.HasOne(e => e.SecondaryImage)
                      .WithMany()
                      .HasForeignKey(e => e.SecondaryImageId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_pet_scans_pet_images_SecondaryImageId");
            });

            // recognition_errors.PetScanId → pet_scans(Id) [Cascade]
            //    An error has no meaning without the scan it belongs to.
            builder.Entity<RecognitionErrors>(entity =>
            {
                entity.ToTable("recognition_errors");

                entity.HasIndex(e => e.PetScanId).HasDatabaseName("IX_recognition_errors_PetScanId");

                entity.HasOne(e => e.PetScan)
                      .WithMany(s => s.RecognitionErrors)
                      .HasForeignKey(e => e.PetScanId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_recognition_errors_pet_scans_PetScanId");
            });

            // health_check_events.PetScanId → pet_scans(Id) [Set Null]
            //    Links an illness scan back to the recognition gate check that verified it.
            builder.Entity<HealthCheckEvent>(entity =>
            {
                entity.HasIndex(e => e.PetScanId).HasDatabaseName("IX_health_check_events_PetScanId");

                entity.HasOne(e => e.PetScan)
                      .WithMany()
                      .HasForeignKey(e => e.PetScanId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_health_check_events_pet_scans_PetScanId");
            });
        }
    }
}
