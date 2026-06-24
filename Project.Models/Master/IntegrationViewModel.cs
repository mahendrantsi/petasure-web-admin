using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Master
{
    public class IntegrationViewModel
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Image {get; set;}
        public bool IsActive { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
