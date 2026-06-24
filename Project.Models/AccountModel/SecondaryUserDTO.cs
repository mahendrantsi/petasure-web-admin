using Project.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AccountModel
{
    public class SecondaryUserDTO
    {
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

        //[Required]
        [Display(Name = "Phone Number")]
        [RegularExpression("^(\\d{10,13})$", ErrorMessage = "Please enter valid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Not a valid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }
}
