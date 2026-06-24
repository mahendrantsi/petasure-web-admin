using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Core.Enum
{
    public enum EnumRefundType
    {
        [Description("Partial Refund")]
        PartialRefund = 1,

        [Description("Full Refund")]
        FullRefund = 2,
    }
}
