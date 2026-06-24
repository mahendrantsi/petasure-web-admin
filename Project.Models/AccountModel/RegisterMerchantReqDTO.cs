using Microsoft.AspNetCore.Http;
using Project.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AccountModel
{
    public class RegisterMerchantReqDTO
    {
        [Display(Name = "Username")]
        [Required]
        [MinLength(5, ErrorMessage = "Username length should be between 5-50 characters")]
        [MaxLength(50, ErrorMessage = "Username length should be between 5-50 characters")]
        [RegularExpression("^[a-zA-Z0-9._@#$%]+$", ErrorMessage = "Invalid UserName")]
        public string Username { get; set; }

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

        // [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression("^(\\d{10,13})$", ErrorMessage = "Please enter valid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Not a valid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain atleast one: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. @$!%*?&)")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirm password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Terms & Conditions")]
        public bool TermsConditions { get; set; }

        //public IFormFile ProfileImage { get; set; }
        //public List<IFormFile> KycDocuments { get; set; }

        [Required(ErrorMessage = "Country Code Required")]
        public int MobileCountryCode { get; set; }= 184;

        [Required(ErrorMessage = "OTP is required")]
        public int OTP { get; set; } 
        public string ReferralCode { get; set; } 
    }

    public class EmailVerificationModel
    {
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Not a valid email")]
        public string Email{ get; set; }
    }
}
