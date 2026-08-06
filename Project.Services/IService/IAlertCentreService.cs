using Project.Models.AdminModel;
using Project.Services.ServiceEntities;
using System;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IAlertCentreService
    {
        Task<ServiceResponse<AlertCentreViewModel>> GetAlerts(AlertFilterViewModel filter = null);
        Task<ServiceResponse<AlertCentreViewModel>> GetAlertsByPage(int pageNumber, int pageSize, string status = null);
        Task<ServiceResponse<AlertDetailViewModel>> GetAlertDetail(Guid alertId);
    }
}
