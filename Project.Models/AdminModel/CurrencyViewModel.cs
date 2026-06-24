using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AdminModel
{
    public class CurrencyViewModel
    {
        public long Id { get; set; }
        public string Enc_Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Is Base Currency")]
        public bool IsBaseCurrency { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        [Display(Name = "Image")]
        [Required]
        public IFormFile Image { get; set; }
        public IFormFile EditImage { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public int FilteredCount { get; set; }
        public int TotalCount { get; set; }
    }
}
