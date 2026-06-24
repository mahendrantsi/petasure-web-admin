using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class Subscriptions
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SubscriptionId { get; set; }
        public string CancellationReason { get; set; }
        public string CancellationReasonComments { get; set; }
        public string ChargeDelay { get; set; }
        public DateTime? CancelledOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ChargeInvervalFrequency { get; set; }
        public int CustomerId { get; set; }
        public DateTime? NextChargeScheduleOn { get; set; }
        public int OrderInvervalFrequency { get; set; }
        public string OrderInvervalUnit { get; set; }
        public string ProductTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string VariantTitle { get; set; }
        public string RechargeProductId { get; set; }
        public string ShopifyProductId { get; set; }
    }
}
