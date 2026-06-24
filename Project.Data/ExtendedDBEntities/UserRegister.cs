using Microsoft.AspNetCore.Http;
using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Data.ExtendedDBEntities
{
    public class UserRegister
    {
        public Guid Id { get; set; }
        public Guid UserID {get;set; }
        public Guid? ParentUserID {get;set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public bool TermsConditions { get; set; }
        public bool Active { get; set; }
        public int CustomerType { get; set; }
        public long BaseCurrencyId { get; set; }
        public decimal BaseCurrencyBalance { get; set; }
        public decimal TokenBalance { get; set; }
        public byte[] QRCode { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid CustomerGuid { get; set; }
        public Dictionary<int, string> error { get; set; }
        public bool IsSuccess { get; set; } = false; 

        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public bool IsMerchant { get; set; }
        public long UserProfileId{ get; set; }
      
        public DateTime? DateOfBirth { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public long CountryID { get; set; }
        public string UserImage { get; set; }
        public string File { get; set; }

        public bool IsDeviceConnected { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public int? MobileCountryCode { get; set; }
        public string Reason { get; set; }
        public string ReferralCode { get; set; }
        public long? ReferredBy{ get; set; }
        public int ShopifyId { get; set; }
        public string ShopifyResponse { get; set; }
        public EnumUserType UserType { get; set; }

    }
}
