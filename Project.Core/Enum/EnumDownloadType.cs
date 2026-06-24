using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Enum
{
    public enum EnumDownloadType
    {   
        [Description("Excel")]
        Excel= 1,
        [Description("CSV")]
        CSV= 2,
        [Description("PDF")]
        PDF= 2,
    }
}
