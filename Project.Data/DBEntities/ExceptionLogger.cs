using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class ExceptionLogger:BaseEntity
    {
        public string Exception { get; set; }
        public string InnerException { get; set; }
    }
}
