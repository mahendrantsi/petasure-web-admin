namespace Project.Models.CommonModel
{
    using System.ComponentModel.DataAnnotations;
    using Project.Core.Enum; 
    using ServiceStack.FluentValidation.Attributes;
    using System;
    using Microsoft.AspNetCore.Http;
    using System.Globalization;
    using Project.Core.Settings;
    using System.ComponentModel.DataAnnotations.Schema;
     
    public class RegisterViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please enter first name")]
        [Display(Name = "First Name")]
        [MaxLength(20, ErrorMessage = "First Name should not exceed 20 characters")]
        [RegularExpression(Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [Display(Name = "Last Name")]
        [MaxLength(20, ErrorMessage = "Last Name should not exceed 20 characters")]
        [RegularExpression(Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid Last  Name")]
        public string LastName { get; set; }

        //[Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter username")]
        [MinLength(5, ErrorMessage = "Username length should be between 5-20 characters")]
        [MaxLength(20, ErrorMessage = "Username length should be between 5-20 characters")]
        [RegularExpression(Setting.UserNameRegex, ErrorMessage = "Invalid username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        [Display(Name = "Phone Number")]
        [RegularExpression("^(\\d{10,11})$", ErrorMessage = "Please enter valid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter Email address")]
        [EmailAddress]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Not a valid email")]
        [Display(Name = "Email")]
        [MaxLength(200,ErrorMessage = "Address should not exceed 200 characters")]
        [MinLength(7)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters and contain atleast one: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. @$!%*?&)")]
        [MaxLength(100, ErrorMessage = "Please enter valid password")]
        [MinLength(8, ErrorMessage = "Please enter valid password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirm password does not match")]
        public string ConfirmPassword { get; set; }
        
        public string Role { get; set; }

        [Required(ErrorMessage ="Check to accept our terms and condition")]
        public bool TermsConditions { get; set; }
        public bool IsChangePassword { get; set; }
     
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Country Code Required")]
        public int? MobileCountryCode { get; set; } = 184;
        public int? OTPCode { get; set; }
        public string ReferralCode { get; set; }
        public int ShopifyId { get; set; }
        public string ShopifyResponse { get; set; }
        public string Address { get; set; }
        public EnumUserType UserType { get; set; }
    } 
     
    public class RegisterViewUserModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Username")]
        [Required]
        [MinLength(5, ErrorMessage = "Username length should be between 5-20 characters")]
        [MaxLength(20, ErrorMessage = "Username length should be between 5-20 characters")]
        [RegularExpression(Setting.UserNameRegex, ErrorMessage = "Invalid username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "please enter first name")]
        [Display(Name = "First Name")]
        [MaxLength(20, ErrorMessage = "First Name should not exceed 20 characters")]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid First Name")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "please enter last name")]
        [Display(Name = "Last Name")]
        [MaxLength(20, ErrorMessage = "Last Name should not exceed 20 characters")]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid Last Name")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Please enter phone number")]
        //[Display(Name = "Phone Number")]
        //[RegularExpression("^(\\d{10,11})$", ErrorMessage = "Please enter valid phone number")]
        //public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "please enter email")]
        [EmailAddress]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Not a valid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters and contain atleast one: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. @$!%*?&)")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirm password does not match")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
        public bool TermsConditions { get; set; }
        public bool IsChangePassword { get; set; }
        
        public bool IsActive { get; set; }
       
        public string BrandLogo { get; set; }

        public bool IsDeviceConnected { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public bool IsProfile{ get; set; }
        public string PhoneNumber { get; set; }


    }

    public class UserDocumentsModel
    {
        public long DocumentID { get; set; }
        public string DocumentTypeName { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string FileLink { get; set; }
    }
    public class ReferralAvailViewModel
    {
        public long Id { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public long CreatedBy { get; set; }
        public long UserId { get; set; }
        public string FromName { get; set; }
        public string FromEmail  { get; set; }
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string ReferralStr { get; set; }
        public string CreatedOnStr { get; set; }

    }
}

