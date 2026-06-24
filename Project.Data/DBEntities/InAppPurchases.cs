using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.ExtendedDBEntities;

namespace Project.Data.DBEntities
{
    public class InAppPurchases: BaseEntity
    {
        [ForeignKey(nameof(User))]
        public Guid? AspnetuserId { get; set; }

        public string TransactionId { get; set; }
        public string ProductId { get; set; }
        public string ProductTitle { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string TransactionReceipt { get; set; }
        public string PurchaseToken { get; set; }
        public bool Acknowledged { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation Property
        public virtual DerivedIdentityUser User { get; set; }
    }
}
