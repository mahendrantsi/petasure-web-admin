namespace Project.Models.AdminModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Project.Core.Enum;
    using Project.Models.ProfileModel;

    public class UserProfileViewModel
    {
        public Guid Id { get; set; }

        public string Enc_Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [RegularExpression("^[A-Za-z0-9_\\+-]+(\\.[A-Za-z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "The Email field is not a valid email address.")]
        public string Email { get; set; }
         
        [Display(Name = "Phone Number")]
        [RegularExpression("^(\\d{10,11})$", ErrorMessage = "Please enter valid phone number")]
        public string PhoneNumber { get; set; }

        public string RoleName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirm password does not match")]
        public string ConfirmPassword { get; set; }

        public SelectList Roles { get; set; }

        public long CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
       
        [MaxLength(200)]
        public string BusinessName { get; set; }
        public string Address { get; set; }
        [Display(Name = "Post Code")]

        [MinLength(6, ErrorMessage = "Postcode must be 6 to 9 character")]
        [MaxLength(9, ErrorMessage = "Postcode must be 6 to 9 character")]
        public string PostCode { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public bool AppNotifications { get; set; }
        public bool SMSNotifications { get; set; }
        public bool EMailNotifications { get; set; }
        public string FCMToken { get; set; }
        public string DeviceType { get; set; }
        public Guid UserId { get; set; }
        public bool IsMerchant { get; set; }
        public string QRCode { get; set; }
        public string ReferralCode { get; set; }
        public Guid CustomerGuid { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<IFormFile> Documents { get; set; }
        public IFormFile UserImageFile { get; set; }
        public List<IFormFile> KycDocuments { get; set; }
        public string UserImage { get; set; }

        private string _StrDateOfBirth { get; set; }
        [Display(Name = "Date Of Birth")]
        [Required]
        [RegularExpression("^([0]?[0-9]|[12][0-9]|[3][01])[./]([0]?[1-9]|[1][0-2])[./]([0-9]{4}|[0-9]{2})$", ErrorMessage = "Please enter valid date of birth")]
        public string StrDateOfBirth
        {
            get
            {
                return _StrDateOfBirth;
            }
            set
            {
                _StrDateOfBirth = value;
                if (_StrDateOfBirth is not null)
                    DateOfBirth = DateTime.ParseExact(_StrDateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }
        [Required(ErrorMessage = "Country Code Required")]
        public int? MobileCountryCode { get; set; }

        public bool IsAccountDeactivationRequested { get; set; } = false;
    }
}
