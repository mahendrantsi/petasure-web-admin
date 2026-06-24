using Microsoft.AspNetCore.Identity;
using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class UserHistory:BaseEntity
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; } 
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? PhoneNumberConfirmedOn { get; set; }

        public Guid? UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public bool IsMerchant { get; set; }
        public Guid ProfleUserID { get; set; }
        public bool ProfileIsActive { get; set; }
        public Guid CustomerGuid { get; set; }
        public string PIN { get; set; }
        public string FCMToken { get; set; }
        public string DeviceType { get; set; }
        public bool AppNotifications { get; set; }
        public bool SMSNotifications { get; set; }
        public bool EMailNotifications { get; set; }

        public string UserImage { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsDeviceConnected { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }

        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
