using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Enum
{
 

    public enum EnumEnquiryStatus
    {
        [Description("Unread")]
        Unread = 1,
        [Description("Read")]
        Read = 2,
        [Description("Deactivated")]
        Deactivated = 3,
        [Description("Rejected")]
        Rejected = 4,

        [Description("Open")]
        Open = 5,
        [Description("Processing")]
        Processing = 6,
        [Description("Completed")]
        Completed = 7,

    }
    public enum EnumEnquiryType
    {
        [Description("Enquiry")]
        Enquiry = 1,
        [Description("Deactivation Request")]
        DeactivationRequest = 2,

    }

    public enum EnumEnquiryViewType
    { 
        All = 1,
        Enquiry = 2,
        Deactivation = 3,
    }

    public enum EnumBusinessViewType
    {
        All = 1,
        Business = 2,
        Franchise = 3,
    }
}
