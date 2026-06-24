using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Models.CommonModel
{
    public class FeeMasterDTO
    {
        public long Id { get; set; }
        public int FeeTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public decimal DefaultFeeAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn
        {
            get
            {
                if (ModifiedOn == null)
                {
                    return DateTime.UtcNow;
                }
                else
                {
                    return ModifiedOn;
                }
            }
            set { ModifiedOn = value; }

        }
        public long ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
    }
}
