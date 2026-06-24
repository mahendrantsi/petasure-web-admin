using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceStack;
using Project.Core.Enum;
using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.ProfileModel
{

    public class ReProfileViewModel
    {
        public long? Id { get; set; }

        //----------------------------PERSONAL INFO START---------------------------\\

        [Required(ErrorMessage = "UserID is require")]
        public long UserID { get; set; }

        [Display(Name = "User Image")]
        public IFormFile UserImage { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(20, ErrorMessage = "First name must be 20 character long")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(20, ErrorMessage = "First name must be 20 character long")]
        public string LastName { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Address")]
        [StringLength(150, ErrorMessage = "The Address field is required.", MinimumLength = 1)]
        [Required(ErrorMessage = "The Address field is required.")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Town")]
        [MaxLength(200)]
        public string Town { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public long AddressCountryID { get; set; } = 184;  // DEFAULT IS UK

        [Required]
        [Display(Name = "Post Code")]
        [MinLength(6, ErrorMessage = "Postcode must be 6 to 9 character")]
        [MaxLength(9, ErrorMessage = "Postcode must be 6 to 9 character")]
        public string PostCode { get; set; }
        //----------------------------PERSONAL INFO END---------------------------\\

        //----------------------------BANK START---------------------------\\

        [Required(ErrorMessage = "The Account No field is required.")]
        [RegularExpression(@"^(?!(?:0{8}|01234567|12345678))(\d){8,8}$", ErrorMessage = "The Account number must have 8 digits")]
        public string AccountNo { get; set; }

        [RegularExpression(@"^[0-9]{2}\s?\-?[0-9]{2}\s?\-?[0-9]{2}$", ErrorMessage = "The Sort code must have 6 digits")]
        [MinLength(6)]
        [MaxLength(6)]
        [Required(ErrorMessage = "The Sort Code field is required.")]
        public string SortCode { get; set; }

        //----------------------------BANK END---------------------------\\

        public bool IsMerchant { get; set; }

        //----------------------------BUSINESS START---------------------------\\
        public IFormFile BusinessImage { get; set; }

        [Required]
        [Display(Name = "Business Name")]
        [MaxLength(200)]
        public string BusinessName { get; set; }


        public string BusinessNumber { get; set; }

     

        [Required]
        public string BusinessAddress { get; set; }
        public string BusinessTown { get; set; }
        [Required]
        public string BusinessPostCode { get; set; }
        [Required]
        public long BusinessCountryID { get; set; } = 184; // DEFAULT IS UK

        //----------------------------BUSINESS START---------------------------\\

        public List<RequireDocs> OnBoardDocument { get; set; }



        private string _StrDateOfBirth { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
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


        [Display(Name = "Country")]
        [MaxLength(200)]
        public string Country { get; set; }

        public string QRCode { get; set; }
        public string baseURL { get; set; }
        [Required]
        public string SelectBusiness { get; set; }
        public long? SearchBusinessID { get; set; }

        [Required]
        public string ParentBusiness { get; set; }
        public long? ParentBusinessID { get; set; }
        public bool Active { get; set; } = true;
    }
}

