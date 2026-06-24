using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Content
{
    public class ContactUsRequestViewModel
    {
       

        [Required]
        [Display(Name = "subject")]        
        [MaxLength(100)]
        public string Subject { get; set; }


        [Required]
        [Display(Name = "message")]
        [MaxLength(5000)]
        public string Message { get; set; }

       
    }
}
