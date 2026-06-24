using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class QRCodeModel
    {
        public string CustomerGuid { get; set; }
        //public decimal? Amount { get; set; }

        public string QRImageURL { get; set; }
    }
}
