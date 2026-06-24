using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class InAppPurchaseViewModel
    {
        public Guid Id { get; set; }
        public Guid AspnetuserId { get; set; }

        public string TransactionId { get; set; }
        public string ProductId { get; set; }
        public string ProductTitle { get; set; }
        public DateTime TransactionDate { get; set; }//will be converted from unix timestamp
        public string TransactionReceipt { get; set; }
        public string PurchaseToken { get; set; }
        public bool Acknowledged { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public class InAppPurchaseInputViewModel
    {
        public Guid AspnetuserId { get; set; }

        public string TransactionId { get; set; }
        public string ProductId { get; set; }
        public string ProductTitle { get; set; }
        public double TransactionDate { get; set; }//will be converted to datetime
        public string TransactionReceipt { get; set; }
        public string PurchaseToken { get; set; }
        public bool Acknowledged { get; set; }

    }
}
