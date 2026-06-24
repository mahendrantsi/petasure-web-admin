namespace Project.Web.Common
{
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationManager
    {
        private static IConfiguration configuataion = null;

        static ConfigurationManager()
        {
            configuataion = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
        }
        public static string GetBaseUrl()
        {
            return configuataion["CustomKeys:BaseUrl"];
        }
        public static string GetCoinHistoryNetworkURL()
        {
            return configuataion["CoinDetail:CoinHistoryNetworkURL"];
        }
        public static string GetAdyenApiKey()
        {
            return configuataion["AdyenDetail:ApiKey"];
        }
        public static string GetAdyenMerchantAccount()
        {
            return configuataion["AdyenDetail:MerchantAccount"];
        }
        public static string GetAdyenClientKey()
        {
            return configuataion["AdyenDetail:ClientKey"];
        }
        public static string GetAdyenReturnUrl()
        {
            return configuataion["AdyenDetail:ReturnUrl"];
        }
    }
}
