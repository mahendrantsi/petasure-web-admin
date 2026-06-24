using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class ContentInfo : BaseEntity
    {

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string Content { get; set; }

        public string Url { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
