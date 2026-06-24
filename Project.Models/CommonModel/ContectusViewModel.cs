using Project.Core.Enum;
using Project.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project.Models.CommonModel
{
    public class ContectusViewModel
    {
        public Guid? ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(40)]
        [RegularExpression(Setting.CheckValidString, ErrorMessage = Setting.CheckValidStringMsg)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Not a valid email")]
        [Display(Name = "Email")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression("^(\\d{10,13})$", ErrorMessage = "Please enter valid phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }

        [Required]
        [MaxLength(1000)]
        [Display(Name = "Description")]
        [RegularExpression(Setting.CheckValidString, ErrorMessage = Setting.CheckValidStringMsg)]
        public string Description { get; set; }

        public EnumEnquiryStatus status { get; set; }

        public string Subject { get; set; }
        public string readByUserName { get; set; }

        public string SendOn { get; set; }
        public string EnquiryType { get; set; }
        public Guid? UserId { get; set; }
        public Guid? EnquiryCode { get; set; }

        public DateTime Createdon { get; set; }


    }
    public class AccountDeactivationRequestModel
    {
        [Required]
        [MaxLength(1000)]
        [Display(Name = "Description")]
        [RegularExpression(Setting.CheckValidString, ErrorMessage = Setting.CheckValidStringMsg)]
        public string Description { get; set; }
    }


    public class AccountDeactivationViewModel
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        [MaxLength(1000)]
        [Display(Name = "Description")]
        [RegularExpression(Setting.CheckValidString, ErrorMessage = Setting.CheckValidStringMsg)]
        public string Description { get; set; }
    }

    public class EnqViewModel
    {
        public Guid EnquiryID { get; set; }
        public Guid UserID { get; set; }
        [RegularExpression(Setting.CheckValidString, ErrorMessage = Setting.CheckValidStringMsg)]
        public string Answer { get; set; }
        public string PlainAnswer { get; set; }
        public bool SendMail { get; set; }
        public EnumEnquiryStatus Status { get; set; }
    }

    public class EnquiryResponseViewModel
    {
        public long Id { get; set; }
        public long EnquiryID { get; set; }
        public string Answer { get; set; }
        public string PlainAnswer { get; set; }
        public bool SendMail { get; set; }
        public EnumEnquiryStatus Status { get; set; }
        public string StatusStr { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
    }
}
