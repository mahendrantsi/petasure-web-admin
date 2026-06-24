using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Enum
{
    public enum EnumRequestType
    {
        [Description("SendMoney")]
        SendMoney = 1,
        [Description("RequestMoney")]
        RequestMoney = 2,
       
    }
}
