using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AccountModel
{
    public class AnonymousRegisterViewModel
    {
        public int OTPCode { get; set; }
        [Required(ErrorMessage = "Please select your bank")]
        public long BankId { get; set; }
        public string TransactionId { get; set; }
        [Required(ErrorMessage = "You need to accept our terms and condition")]
        public bool TermsConditions { get; set; }
        public Guid data { get; set; }
        public string PhoneNo { get; set; }
    }

    public class AnonymousPaymentConfirmViewModel
    {
        public string TransactionId { get; set; }
        public Guid data { get; set; }
        public string Amount { get; set; }
        public string PayeeName { get; set; }
        public string PayeePhoneNumber { get; set; }
        public string PayeeInstitutionName { get; set; }
        public string AccountNo { get; set; }
        public string SortCode { get; set; }
    }

    public class AnonymousUserBankViewModel
    {
        [Required(ErrorMessage = "Please select bank")]
        public long BankId { get; set; }
        public bool Selected { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string IconUrl { get; set; }
        public string LogoUrl { get; set; }
        [Required(ErrorMessage = "Please enter account no")]
        [RegularExpression(@"^(?!(?:0{8}|01234567|12345678))(\d){8,8}$", ErrorMessage = "The Account number must have 8 digits")]
        public string AccountNo { get; set; }
        [Required(ErrorMessage = "Please enter sort code")]
        [RegularExpression(@"^[0-9]{2}\s?\-?[0-9]{2}\s?\-?[0-9]{2}$", ErrorMessage = "The Sort code must have 6 digits")]
        public string SortCode { get; set; }
        public long userID { get; set; }
        [Required(ErrorMessage = "Please enter account holder name")]
        [MaxLength(50)]
        public string accountName { get; set; }
        public string TransactionId { get; set; }

    }


    public class Pay2QRViewModel
    {
        [Required(ErrorMessage = "Please select bank")]
        public long BankId { get; set; }
        public string PayeeImg { get; set; }
        public string PayeeName { get; set; }
        public Guid payeeGuid { get; set; }
        public string Currency { get; set; } = "£";

        [Required(ErrorMessage = "Please enter amount")]
        [Range(0.01, 15000.00, ErrorMessage = "Amount must be greater than zero and less than equal to 15000.")]
        //[RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount can have up to two decimal places.")]
        [RegularExpression(@"^(0?(\.\d{1,2})?|\d+(\.\d{1,2})?)$", ErrorMessage = "Amount can have up to two decimal places.")]
        public decimal? Amount { get; set; }
         
        [Required(ErrorMessage = "Please enter description")]
        [MinLength(6, ErrorMessage = "Please add minimum 6 characters.")]
        [MaxLength(18, ErrorMessage = "Description should not be more than 18 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        [Display(Name = "Phone Number")]
        [RegularExpression("^(\\d{10,11})$", ErrorMessage = "Please enter valid phone number")]
        public string PhoneNo { get; set; }
        public int? OTPCode { get; set; }
    }


}
