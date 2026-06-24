using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class UserOTPVerifyResponseViewModel
    {
        public bool isOTPValid { get; set; }
        public bool MobileNumberVerified { get; set; }
        public bool EmailVerified { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
    }
}
