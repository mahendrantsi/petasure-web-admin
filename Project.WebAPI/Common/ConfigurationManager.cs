using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WebAPI.Common
{
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
    }
}
