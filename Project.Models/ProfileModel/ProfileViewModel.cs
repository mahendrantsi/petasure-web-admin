using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceStack;
using Project.Core.ActionFilter;
using Project.Core.Enum;
using Project.Core.Settings;
using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ServiceStack.LicenseUtils;

namespace Project.Models.ProfileModel
{
    public class ProfileViewModel
    {
        public long? Id { get; set; }

        //----------------------------PERSONAL INFO START---------------------------\\

        [Required(ErrorMessage = "UserID is require")]
        public long UserID { get; set; }

        [Display(Name = "User Image")]
        public IFormFile UserImage { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [Display(Name = "First Name")]
        [MaxLength(20, ErrorMessage = "First name must be 20 character long")]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter last name")]
        [Display(Name = "Last Name")]
        [MaxLength(20, ErrorMessage = "First name must be 20 character long")]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid Last name")]
        public string LastName { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }
         
        [Display(Name = "Address")]
        [StringLength(150, ErrorMessage = "The Address field is required.", MinimumLength = 1)]
        [Required(ErrorMessage = "Please enter Address")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public long AddressCountryID { get; set; } = 184;  // DEFAULT IS UK

        [Required(ErrorMessage = "Please enter Post Code")]
        [Display(Name = "Post Code")]
        [MinLength(6, ErrorMessage = "Postcode must be 6 to 9 character")]
        [MaxLength(9, ErrorMessage = "Postcode must be 6 to 9 character")]
        public string PostCode { get; set; }
       
    }
   
}
