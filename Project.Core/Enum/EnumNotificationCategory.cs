using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Enum
{
    public enum EnumNotificationCategory
    {
        [Description("Transaction")]
        Transaction = 1,
        [Description("Chat")]
        Chat = 2,
    }
}
