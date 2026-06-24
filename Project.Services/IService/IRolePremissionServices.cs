using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IRolePremissionServices
    {
        Task<ServiceResponse<Dictionary<string, bool>>> GetStaffPermission(Guid userID);
    }
}
