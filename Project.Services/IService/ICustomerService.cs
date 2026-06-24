
namespace Project.Services.IService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AspNetCore.ServiceRegistration.Dynamic;
    using Project.Models.AdminModel;
    using Project.Models.CommonModel;
    using Project.Services.ServiceEntities;

    [ScopedService]
    public interface ICustomerService
    {
        ServiceResponse<List<UserViewModel>> GetCustomerInformation(JQueryDataTableModel requestParam);

        ServiceResponse<UserViewModel> GetCustomerInformationById(long customerId);

        ServiceResponse<UserProfileViewModel> GetCustomerProfileById(long id);

        Task<ServiceResponse<UserProfileViewModel>> Edit(UserProfileViewModel model);
    }
}
