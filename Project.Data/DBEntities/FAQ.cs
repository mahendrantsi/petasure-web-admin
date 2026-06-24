using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class FAQ: BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }=false;
        public int Order { get; set; }
    }
}
