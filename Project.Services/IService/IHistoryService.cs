using Project.Data.ExtendedDBEntities;
using Project.Models.CommonModel;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IHistoryService
    {
        Task<ServiceResponse<UserHistoryViewModel>> SaveUserHistory(UserRegister model);
        Task<ServiceResponse<UserHistoryViewModel>> SaveUserProfileHistory(UserRegister model, Guid userId);

    }
}
