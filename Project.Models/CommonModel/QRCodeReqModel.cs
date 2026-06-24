using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class QRCodeReqModel
    {
        [Required]
        public string CustomerGuid { get; set; }
        public decimal? Amount { get; set; } 
        public long? BranchID { get; set; } 
    }


    public class QrRequestModel
    { 
        public long userID { get; set; }
        public decimal? Amount { get; set; }
    }


    public class QRModel
    {
        //[Required]
        //public string CustomerGuid { get; set; }
        public decimal? Amount { get; set; }
    }
}
