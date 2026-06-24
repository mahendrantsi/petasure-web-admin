using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Master
{
    public class CountryCodesViewModel
    {
        public long Id { get; set; }
        public string CountryName { get; set; }
        public string ShortCode { get; set; }
        public string Code { get; set; }
        public string DialCode { get; set; }
        public bool IsActive { get; set; }

        public string ImageUrl { get; set; }

    }
}
