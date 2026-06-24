
namespace Project.Models.AdminModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ContentViewModel
    {
        public long Id { get; set; }

        public string Enc_Id { get; set; }

        public string ContentType { get; set; }

        public string ModifiedBy { get; set; }
         
        public DateTime? ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
