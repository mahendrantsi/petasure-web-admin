namespace Project.Models.AccountModel
{
    using System.ComponentModel.DataAnnotations; 
    using ServiceStack.FluentValidation.Attributes;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Project.Core.Enum;
    using System.ComponentModel;
    using Project.Core.Settings;
    using Project.Models.Validations;

    [Validator(typeof(LoginReqDTOValidator))]
    public class LoginReqDTO
    {
        [Required]
        //[EmailAddress]
        //[RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Not a valid email")]
        //[RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please enter valid username")]
        //[MinLength(5, ErrorMessage = "Username length should be between 5-50 characters")]
        //[MaxLength(50, ErrorMessage = "Username length should be between 5-50 characters")]
        ////[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username must be alphanumeric.")]
        //[RegularExpression("^[a-zA-Z0-9._@#$%]+$", ErrorMessage = "Username must be in alphanumeric and can contains these symbols ._@#$%")]
        [RegularExpression(Setting.UserNameRegex, ErrorMessage = "Invalid username")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain atleast one: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. @$!%*?&)")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string Password { get; set; }
        //[Required]
        [Display(Name = "FCM Token")]
        public string FCMToken { get; set; }
        //[Required]
        [Display(Name = "Device Type")]
        public string DeviceType { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        //[DefaultValue("User")]
        //public string UserType { get; set; } = EnumRole.User.ToString();


    }

    public class LoginWith2faViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }

    }

    public class TwoFactorAuthenticationViewModel
    {
        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        public bool Is2faEnabled { get; set; }
    }
    public class ShowRecoveryCodesViewModel
    {
        public string[] RecoveryCodes { get; set; }
        public long MyProperty { get; set; }
    }
    public class LoginWithRecoveryCodeViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode { get; set; }
    }
}
