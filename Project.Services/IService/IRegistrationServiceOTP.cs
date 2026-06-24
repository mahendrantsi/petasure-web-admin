using Project.Data.DBEntities;
using Project.Models.CommonModel;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IRegistrationServiceOTP
    {
        Task<ServiceResponse<RegistrationOTP>> GenerateOTP(OTPViewModel model);
        Task<ServiceResponse<string>> VerifyOTP(RegisterViewModel model);
        Task<ServiceResponse<string>> GetOTPExipreTime(string phoneNo);
        Task<ServiceResponse<string>> CheckAnonymousUser(string PhoneNumber, string TransactionId);
    }
}
