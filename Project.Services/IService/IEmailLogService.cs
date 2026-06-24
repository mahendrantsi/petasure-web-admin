using Project.Models.CommonModel;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IEmailLogService
    {
        Task<ServiceResponse<EmailLogViewModel>> Create(EmailLogViewModel emailLogViewModel);
    }
}
