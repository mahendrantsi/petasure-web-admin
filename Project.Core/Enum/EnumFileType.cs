using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Enum
{
    public enum EnumFileType
    {
        [Description("Profile")]
        Profile = 1,
        [Description("KybDocuments")]
        KybDocuments = 2,
        [Description("KycDocuments")]
        KycDocuments = 3,
        [Description("BusinessProfile")]
        BusinessProfile = 4,
        [Description("RewardLogo")]
        RewardLogo = 5,
        [Description("RewardBanner")]
        RewardBanner = 6,
    }
   
}
