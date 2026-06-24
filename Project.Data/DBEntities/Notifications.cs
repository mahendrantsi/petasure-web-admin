using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class Notifications:BaseEntity
    {
        public long UserID { get; set; }
        public string Title { get; set; }
        public string body { get; set; }
        public EnumNotificationCategory Type { get; set; }
        public long? TransactionID{ get; set; }
        public bool IsRead { get; set; } =false;
    }
}
