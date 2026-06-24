using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class UserHistoryViewModel
    {
        public long UserId { get; set; }
        [Display(Name = "UserName")]
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

        public long? UserProfileId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        public bool IsMerchant { get; set; }
        [Required(ErrorMessage = "UserID is require")]
        public long ProfleUserID { get; set; }
        public bool ProfileIsActive { get; set; }
        public Guid CustomerGuid { get; set; }
        public string PIN { get; set; }
        public string FCMToken { get; set; }
        public string DeviceType { get; set; }
        public bool AppNotifications { get; set; }
        public bool SMSNotifications { get; set; }
        public bool EMailNotifications { get; set; }

    }


    public class UserHistoryDetailsViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Email Confirmed")]
        public bool EmailConfirmed { get; set; }
        [DisplayName( "Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName( "Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }
        [DisplayName( "Phone Number Confirmed On")]
        public DateTime? PhoneNumberConfirmedOn { get; set; }
        [DisplayName( "KYC Status")]
        public string IsKyc { get; set; }
        [DisplayName( "Active")]
        public string IsActive { get; set; }
        public string IsDeleted { get; set; }
        [DisplayName( "First Name")]
        public string FirstName { get; set; }
        [DisplayName( "Last Name")]
        public string LastName { get; set; }
        [DisplayName( "Business Name")]
        public string BusinessName { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
        [DisplayName("Post Code")]
        public string PostCode { get; set; }
        [DisplayName("Is Merchant")]
        public bool IsMerchant { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Created On")]
        public DateTime? CreatedOn { get; set; }
       
        [DisplayName( "Profile Picture")]
        public string UserImage { get; set; }
        [DisplayName( "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName( "Mfa Device Connected")]
        public bool IsDeviceConnected { get; set; }
        [DisplayName("Town")]
        public string Town { get; set; }
        [DisplayName("Country")]
        public string Country { get; set; }
        [DisplayName( "MFA Enabled")]
        public bool TwoFactorEnabled { get; set; }

    }

    public class UserHistoryLogViewModel
    {
        public string UserName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Email Confirmed")]
        public bool EmailConfirmed { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }
        [DisplayName("Phone Number Confirmed On")]
        public DateTime? PhoneNumberConfirmedOn { get; set; }
        [DisplayName("KYC Status")]
        public string IsKyc { get; set; }
        [DisplayName("Active")]
        public string IsActive { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Business Name")]
        public string BusinessName { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
        [DisplayName("Post Code")]
        public string PostCode { get; set; }
        [DisplayName("Is Merchant")]
        public bool IsMerchant { get; set; }
        [DisplayName("Profile Picture")]
        public string UserImage { get; set; }
        [DisplayName("Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Mfa Device Connected")]
        public bool IsDeviceConnected { get; set; }
        [DisplayName("Town")]
        public string Town { get; set; }
        [DisplayName("Country")]
        public string Country { get; set; }
        [DisplayName("MFA Enabled")]
        public bool TwoFactorEnabled { get; set; }

    }
    
}
