using Project.Models.Master;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IIntegrationService
    {
        ServiceResponse<List<IntegrationViewModel>> Get();
        Task<ServiceResponse<IntegrationViewModel>> Create(IntegrationViewModel model);
        Task<ServiceResponse<IntegrationViewModel>> Update(IntegrationViewModel model);
        Task<ServiceResponse<IntegrationViewModel>> Delete (Guid Id);
    }
}
