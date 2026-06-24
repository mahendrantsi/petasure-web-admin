using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.ServiceHelper
{
    public static class OTPHelper
    {
        public static string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);

                sOTP += saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
            }

            return sOTP;
        }
    }
}
