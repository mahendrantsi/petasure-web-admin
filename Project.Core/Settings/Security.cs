using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Settings
{

    public class MySettingsModel
    {
        public string DbConnection { get; set; }
        public string Email { get; set; }
        public string SMTPPort { get; set; }
    }


    public class Security
    {
        // / <summary>
        // / appSettings.
        // / </summary>
        private readonly IOptions<MySettingsModel> appSettings;

        public Security(IOptions<MySettingsModel> app)
        {
            appSettings = app;
        } 

    }
}
