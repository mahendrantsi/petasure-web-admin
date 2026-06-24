using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class ContentMaster : BaseEntity
    {
        [Required]
        public string ContentType { get; set; }

        [Required]
        public string HTMLContent { get; set; }

        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

        public long? ModifiedBy { get; set; }
    }
}
