using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class ResetPasswordViewModel
    {
        

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain atleast one: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. @$!%*?&)")]
        [MaxLength(100, ErrorMessage = "Please enter valid password")]
        [MinLength(8, ErrorMessage = "Please enter valid password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirm​ password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Data { get; set; }
    }


    public class ForgotPasswordResponseModel
    {
        public string UserType { get; set; }
        public string CallbackURL { get; set; }
    }
}
