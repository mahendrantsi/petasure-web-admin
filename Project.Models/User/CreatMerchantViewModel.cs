using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceStack;
using Project.Core.Enum;
using Project.Core.Settings;
using Project.Data.DBEntities;
using Project.Models.CommonModel;
using Project.Models.ProfileModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.User
{
    public class CreatMerchantViewModel
    {
        public RegisterViewUserModel registerViewUserModel { get; set; }

        //----------------------------PERSONAL INFO START---------------------------\\

        public long UserID { get; set; }

        [Display(Name = "User Image")]
        public IFormFile UserImage { get; set; }
        
        //----------------------------PERSONAL INFO END---------------------------\\

        //----------------------------BANK START---------------------------\\
        //[Required]

        [Required(ErrorMessage = "Please enter valid account no")]
        [RegularExpression(@"^(?!(?:0{8}|01234567|12345678))(\d){8,8}$", ErrorMessage = "The Account number must have 8 digits")]
        public string AccountNo { get; set; }

        [Required(ErrorMessage = "Please enter valid sort code")]
        [RegularExpression(@"^[0-9]{2}\s?\-?[0-9]{2}\s?\-?[0-9]{2}$", ErrorMessage = "The Sort code must have 6 digits")]
        [MinLength(6, ErrorMessage = "The Sort code must have 6 digits")]
        [MaxLength(6, ErrorMessage = "The Sort code must have 6 digits")]        
        public string SortCode { get; set; }

        [Required(ErrorMessage = "Please select your bank")]
        public long BankId { get; set; }

        //----------------------------BANK END---------------------------\\

        public bool IsMerchant { get; set; }

        //----------------------------BUSINESS START---------------------------\\
        public IFormFile BusinessImage { get; set; }

        [Required(ErrorMessage = "please enter business name")]
        [Display(Name = "Business Name")]
        [MaxLength(100)]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid business name")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = "please enter Company number")]
        [MaxLength(8)]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid business number")]
        public string BusinessNumber { get; set; }

        [Required(ErrorMessage = "please enter business category name")]
        [MaxLength(50)]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid business category")]
        public string BusinessCategoryName { get; set; }

        [Required(ErrorMessage = "please enter business address")]
        [MaxLength(150)]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid business address")]
        public string BusinessAddress { get; set; }
        [MaxLength(20)]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid town")]
        public string BusinessTown { get; set; }
        [Required(ErrorMessage = "please enter business postcode")]
        [MinLength(6, ErrorMessage = "Postcode must be 6 to 9 character")]
        [MaxLength(9, ErrorMessage = "Postcode must be 6 to 9 character")]
        [RegularExpression(pattern: Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid postcode")]
        public string BusinessPostCode { get; set; }
        [Required(ErrorMessage = "select business country")]
        public long BusinessCountryID { get; set; } = 184; // DEFAULT IS UK

        //----------------------------BUSINESS START---------------------------\\

        public List<RequireDocs> OnBoardDocument { get; set; }


        [Display(Name = "Country")]
        [MaxLength(200)]
        public string Country { get; set; }

        public string QRCode { get; set; }
        public string baseURL { get; set; }


        [Required(ErrorMessage = "Please enter parent business name")]
        public string ParentBusiness { get; set; }
        public long? ParentBusinessID { get; set; }
        public bool Active { get; set; } = true;

        public string UserImageStr { get; set; }
        public string BusinessImageStr { get; set; }
        public string ParentBusinessName { get; set; }
        public string Reason { get; set; }
    }


}
