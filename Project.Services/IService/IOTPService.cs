using SmartPay.Models.CommonModel;
using SmartPay.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.IService
{
    public interface IOTPService
    {
        Task<ServiceResponse<UserOTPResponseViewModel>> SendOTP(UserOTPRequestViewModel userOTPReqViewModel);
        Task<ServiceResponse<UserOTPResponseViewModel>> ResendOTP(UserOTPRequestViewModel userOTPReqViewModel);
        Task<ServiceResponse<UserOTPVerifyResponseViewModel>> VerifyOTP(UserOTPVerifyRequestViewModel UserOTPVerifyRequestViewModel);
    }
}
