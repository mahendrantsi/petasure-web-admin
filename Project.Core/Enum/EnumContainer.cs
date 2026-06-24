using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Enum
{
    public enum EnumContainer
    {
        [Description("User Documents")]
        UserDocuments = 1,
        [Description("Profile Image")]
        ProfileImage = 2,
        [Description("Brand Logo")]
        BrandLogo = 3,
        [Description("Root")]
        Root = 4,
        [Description("ChatDocument")]
        ChatDocument = 5,
        [Description("EmailTemplate")]
        EmailTemplate = 6,
        [Description("BusinessPicture")]
        BusinessPicture = 7,
        [Description("BusinessDocument")]
        BusinessDocument = 8, 
        [Description("RewardCatalog")]
        RewardCatalog = 9,
    }
}
