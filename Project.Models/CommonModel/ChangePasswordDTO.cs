namespace Project.Models.CommonModel
{
    using System.ComponentModel.DataAnnotations;
    using Project.Models.Validations;
    using ServiceStack.FluentValidation.Attributes;
    using Project.Core.Settings;

    public class ChangePasswordDTO
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "The Current Password field is required.")]
        [RegularExpression(Setting.CheckValidString, ErrorMessage = Setting.CheckValidStringMsg)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[#$@!%&*?])[A-Za-z0-9#$@!%&*?]{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain atleast one: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. @$!%*?&)")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirm​ password do not match.")]
        [Display(Name = "Confirm Password")]
        [RegularExpression(Setting.CheckValidString, ErrorMessage = Setting.CheckValidStringMsg)]
        public string ConfirmNewPassword { get; set; }
    }
}
