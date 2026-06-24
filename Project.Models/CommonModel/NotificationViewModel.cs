using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class NotificationViewModel
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public string Title { get; set; }
        public string body { get; set; }
        public string Category { get; set; }
        public string TransactionCode { get; set; }
        public long? TransactionID { get; set; }
        public DateTime CreatedOn { get; set; }

    }

    public class NotificationCountViewModel
    {
        public bool HasNotification { get; set; }
        public int Count { get; set; }

    }
}
