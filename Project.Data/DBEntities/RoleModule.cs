using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class RoleModule : BaseEntity
    {
        public string Name{ get; set; }
        public string Desc{ get; set; }
        public long? ParentModule{ get; set; }
    }
}
