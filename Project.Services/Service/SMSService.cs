using Microsoft.Extensions.Configuration;
using SmartPay.Services.IService;
using SmartPay.Services.Resources;
using SmartPay.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SmartPay.Services.Service
{
    public class SMSService : BaseService, ISMSService
    {
        private readonly IConfiguration _configuration;

        public SMSService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> SendSMS(string PhoneNumber, String Message)
        {
            var response = new ServiceResponse<string>();
            try
            {
                string accountSid = _configuration["Twilio:AccountSid"];
                string authToken = _configuration["Twilio:AuthToken"];
                string fromNumber = _configuration["Twilio:FromNumber"];

                TwilioClient.Init(accountSid, authToken);

                MessageResource.Create(
                    body: Message,
                    from: new Twilio.Types.PhoneNumber(fromNumber),
                    to: new Twilio.Types.PhoneNumber(PhoneNumber)
                );

                response = SetResultStatus("SMS Sent", Messages_Resources.Success, true);
            }
            catch (Exception ex)
            {
                response = SetResultStatus(ex.InnerException.Message, Messages_Resources.Error, false);
            }
            return response;
        }
    }
}
