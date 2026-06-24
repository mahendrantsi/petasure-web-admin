namespace Project.Models.AdminModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ContentDetailViewModel
    {
        public long Id { get; set; }

        public string Enc_Id { get; set; }

        [Required]
        [Display(Name = "Content Type")]
        public string ContentType { get; set; }

        //[Required(ErrorMessage ="Content is required")]
        [Display(Name = "Content")] 
        public string HTMLContent { get; set; }

        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
