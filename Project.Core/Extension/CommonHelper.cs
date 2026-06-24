using Newtonsoft.Json;
using Project.Core.Settings;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project.Core.Extension
{
    public static class CommonHelper
    {
        public static string  TryDecodeBase64String(this string base64Input)
        {
            
            // Check if the string is null or empty
            if (string.IsNullOrWhiteSpace(base64Input))
            {
                return null;
            }

            base64Input = base64Input.Replace("-", "+").Replace("_", "/");
            // Base64 strings should have a length that's a multiple of 4
            while (base64Input.Length % 4 != 0)
            {
                base64Input += "=";  // pad with '='
            }

            try
            { 
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64Input)); 
            }
            catch (FormatException)
            {
                // The provided string is not a valid Base64 string
                return null;
            }
        }
        public static string DecodeJWT(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt)) { return null; }
            string[] parts = jwt.Split('.');
            if (parts.Length != 3)
            {
                Console.WriteLine("Invalid JWT token");
                return null;
            }

            var Header=TryDecodeBase64String(parts[0]);
            var Payload= TryDecodeBase64String(parts[1]);
            // The third part is the signature, which isn't directly decodable to a meaningful string.

            return JsonConvert.SerializeObject(new { Header, Payload });
        }

        public static string TrimMobileNo(string mobileNo)
        {
            Regex regexIsMobileNo = new Regex(Setting.IsMobileNo);
            if ((regexIsMobileNo.Match(mobileNo)).Success)
            {
                string cleanedPhoneNumber = Regex.Replace(mobileNo, @"\D", "");
                string pattern = @"(\d{10})$";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(cleanedPhoneNumber);
                if (match.Success)
                    return match.Value;
            }
            return mobileNo;
        }
    }
}
