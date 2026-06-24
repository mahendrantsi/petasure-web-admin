using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Model
{
    public class InAppValidateResponse
    {
        public bool IsValid { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
