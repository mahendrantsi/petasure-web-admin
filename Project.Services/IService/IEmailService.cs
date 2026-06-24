using Project.Core.Enum;
using Project.Services.Service;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IEmailService
    {
        Task SendLinkForgotAsync(string userName, string ResetPasswordLink);
        Task SendLinkCreateAsync(string userName, string ResetPasswordLink);
        Task SendSecondaryUserEmailAsync(string emailID, string passwordString);

        Task SendMissingPetSupportEmail(string emailID, string petname);
        Task SendMissingPetAcknowledgeEmail(string emailID);
        Task SendFoundMissingPetSupportEmail(string emailID, string petname, string phone);


    }
}
