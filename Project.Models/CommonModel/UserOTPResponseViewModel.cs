using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class UserOTPResponseViewModel
    {
        public uint OTP { get; set; }
        public uint Tries { get; set; }
        public Nullable<System.DateTime> ExpiresAt { get; set; }
    }
}
