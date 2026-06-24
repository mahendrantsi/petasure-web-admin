using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Content
{
    public class FaqViewModel
    {
        public Guid Id { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }
      
        public int Order { get; set; }
    }
}
