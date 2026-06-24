using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class StaffRole : BaseEntity
    {
        public string Name { get; set; }
        public long BusinessID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
