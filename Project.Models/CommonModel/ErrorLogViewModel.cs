using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class ErrorLogViewModel
    {
        public string ApplicationUserId { get; set; }
        public string UserUuid { get; set; }
        public string Bank { get; set; }
        public string Error { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorDescription { get; set; }
        public string Transaction { get; set; }
        public string UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string  PayerName{ get; set; }
    }
    public class TransactionErrorsViewModel
    {
        public string TransactionCode { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public string PayerName { get; set; }
        public string IsSuccess { get; set; }
        public string CreatedOn { get; set; }
        public string Email{ get; set; }
        public string Type{ get; set; }
    }
}
