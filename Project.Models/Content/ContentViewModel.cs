using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project.Models.Content
{
    public class ContentViewModel
    {
        public Guid? Id { get; set; }
        
        [Required]
        [Display(Name = "Name")]
        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Description")] 
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Content ")]
        public string Content { get; set; }


        public DateTime ModifyOn{ get; set; }
        public long ModifyBy { get; set; }
        public Guid CreatedBy { get; set; }

        public string ModifyUserName { get; set; }
    }
}
