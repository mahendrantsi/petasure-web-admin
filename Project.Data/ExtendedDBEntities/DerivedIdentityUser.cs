namespace Project.Data.ExtendedDBEntities
{
    using Project.Core.Enum;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.ComponentModel.DataAnnotations;

    //  Add profile data for application users by adding properties to the WebApplication12User class
    public class DerivedIdentityUser : IdentityUser<Guid>
    {
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public Guid? ParentUserID { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsDeviceConnected { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? PhoneNumberConfirmedOn { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public int ShopifyId { get; set; }
        public string ShopifyResponse { get; set; }
        public string ImagePath { get; set; }
        public EnumUserType UserType { get; set; }

        public string Address { get; set; }
        public string? LicenseNumber { get; set; }
        public string? IssuingAuthority { get; set; }
    }
}
