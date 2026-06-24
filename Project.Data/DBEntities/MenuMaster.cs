using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Data.DBEntities
{
   public class MenuMaster:BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string MenuName { get; set; }
        public int ParentId { get; set; }
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }
        [StringLength(100)]
        public string Action { get; set; }
     
        public string Controller { get; set; }
       
        public string Url { get; set; }
        public bool? IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string Icon { get; set; }
        public bool? IsDefault { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
   
}
