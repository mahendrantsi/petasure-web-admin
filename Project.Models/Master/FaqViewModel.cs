using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Settings;

namespace Project.Models.Master
{
    public class FAQViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(400,ErrorMessage ="Question must not exceed 400 characters")]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
         
        public string CreatedOnStr { get; set; }
        public string CreatedByStr { get; set; }

        public int Order { get; set; }
    }
}
