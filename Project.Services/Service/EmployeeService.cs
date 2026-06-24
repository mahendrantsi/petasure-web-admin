
using SmartPay.Data.DBEntities;
using SmartPay.Services.IService;
using SmartPay.Services.ServiceEntities;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SmartPay.Services.Service
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly EmailConfig ec;

        public EmployeeService(IOptions<EmailConfig> emailConfig)
        {
            this.ec = emailConfig.Value;
        }

    }
}
