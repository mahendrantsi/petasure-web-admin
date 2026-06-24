using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Master
{
    public class DocumentTypeViewModel
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
    }
}
