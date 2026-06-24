using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Links
{
    public class LinkViewModel
    {
        public Guid Id { get; set; }
        public string url { get; set; }
        public long? userID { get; set; }
        public long? transactionID { get; set; }
        public bool IsExpired { get; set; }
     

    }
}
