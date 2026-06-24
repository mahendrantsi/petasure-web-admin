using System.Reflection.Metadata;

namespace Project.Core.Settings
{
    public static class Setting
    {

        public static int AppId { get; set; }

        public static string SaltVersion { get; } = "1";

        public static string PoundSign { get; } = "£";

        public const string WhiteSpaceRegex = "^\\S(.*\\S)?$";
        public const string UserNameRegex = "^[a-zA-Z0-9._@#$%]+$";
        public const string IsMobileNo = "^(\\+?\\d{1,3}?[-.\\s]?)?(\\(?\\d{1,4}?\\)?[-.\\s]?)?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$";
        //public const string CheckValidString = @"(<script[^>]*>.*?</script>)|(<.*?javascript:.*?>)|(<.*?on\w+=.*?>)|(%3Cscript.*?%3E)";
        public const string CheckValidString = @"^(?!.*(<script.*?>.*?</script>|<.*?on\w+\s*=\s*['""]?.*?|<.*?javascript:.*?|%3Cscript.*?%3E)).*$";

        public const string CheckValidStringMsg = "Input value contains some malicious characters";

        public static string[] _validImgExtensions = { ".png", ".jpg", ".jpeg" };


        public static string MaskSensitiveInfo(this string input)
        {
            int visibleDigits = 2;
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            int length = input.Length;

            if (visibleDigits >= length)
            {
                // If the number of visible digits is greater than or equal to the length of the input,
                // return the input as it is.
                return input;
            }

            string maskedPart = new string('*', length - visibleDigits);

            return maskedPart + input.Substring(length - visibleDigits);
        }
    }
}
