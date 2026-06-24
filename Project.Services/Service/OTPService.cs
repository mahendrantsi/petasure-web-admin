using SmartPay.Core.Extension;
using SmartPay.Data;
using SmartPay.Data.DBEntities;
using SmartPay.Data.ExtendedDBEntities;
using SmartPay.Models.CommonModel;
using SmartPay.Persistence.UOW;
using SmartPay.Services.IService;
using SmartPay.Services.ServiceEntities;
using SmartPay.Services.ServiceHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SmartPay.Services.Service
{
    public class OTPService : BaseService, IOTPService
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISMSService _smsService;

        public OTPService(IAccountService accountService, IUnitOfWork unitOfWork, ISMSService smsService)
        {
            this._accountService = accountService;
            this._unitOfWork = unitOfWork;
            this._smsService = smsService;
        }

        public async Task<ServiceResponse<UserOTPResponseViewModel>> SendOTP(UserOTPRequestViewModel userOTPReqViewModel)
        {
            var user = await this._accountService.GetByEmailAsync(userOTPReqViewModel.Email);
            ServiceResponse<UserOTPResponseViewModel> objReturn = new ServiceResponse<UserOTPResponseViewModel>();
            UserOTPResponseViewModel userOTPResponseViewModel = new UserOTPResponseViewModel();
            try
            {
                String[] allowedChars = new string[] { "1", "2", "3", "4", "5", "6", "7" };
                userOTPResponseViewModel.OTP = Convert.ToUInt32(OTPHelper.GenerateRandomOTP(6, allowedChars));
                userOTPResponseViewModel.ExpiresAt = DateTime.Now.AddMinutes(10);
                userOTPResponseViewModel.Tries = 0;
                if (await StoreOTP(userOTPResponseViewModel, user))
                {
                    objReturn = this.SetResultStatus<UserOTPResponseViewModel>(userOTPResponseViewModel, "OTP sent successfully.", true);
                    _smsService.SendSMS(user.PhoneNumber,$"{userOTPResponseViewModel.OTP} is your verification code for SmartPay");
                }
                else
                {
                    objReturn = this.SetResultStatus<UserOTPResponseViewModel>(userOTPResponseViewModel, "Fail to send OTP.", true);
                }
            }
            catch
            {
                objReturn = this.SetResultStatus<UserOTPResponseViewModel>(null, "Fail to send OTP.", false);
            }
            return objReturn;
        }

        private async Task<bool> StoreOTP(UserOTPResponseViewModel userOTPResponseViewModel, DerivedIdentityUser user)
        {
            bool status;
            UserOTP userOTP;
            try
            {
                userOTP = this._unitOfWork.UserOTPRepository.Find(x => x.UserId == user.Id).FirstOrDefault();

                if (userOTP != null)
                {
                    userOTP.OTP = userOTPResponseViewModel.OTP;
                    userOTP.ExpiresAt = userOTPResponseViewModel.ExpiresAt;
                    userOTP.CreatedOn = DateTime.Now;
                    this._unitOfWork.UserOTPRepository.UpdateEntity(userOTP);
                }
                else
                {
                    userOTP = new UserOTP
                    {
                        UserId = user.Id,
                        OTP = userOTPResponseViewModel.OTP,
                        Tries = userOTPResponseViewModel.Tries,
                        ExpiresAt = userOTPResponseViewModel.ExpiresAt,
                        CreatedOn = DateTime.Now
                    };
                    this._unitOfWork.UserOTPRepository.Add(userOTP);
                }

                status = await this._unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public async Task<ServiceResponse<UserOTPResponseViewModel>> ResendOTP(UserOTPRequestViewModel userOTPReqViewModel)
        {
            var user = await this._accountService.GetByEmailAsync(userOTPReqViewModel.Email);
            ServiceResponse<UserOTPResponseViewModel> objReturn = new ServiceResponse<UserOTPResponseViewModel>();
            UserOTPResponseViewModel userOTPResponseViewModel = new UserOTPResponseViewModel();

            try
            {
                String[] allowedChars = new string[] { "1", "2", "3", "4", "5", "6", "7" };
                userOTPResponseViewModel.OTP = Convert.ToUInt32(OTPHelper.GenerateRandomOTP(6, allowedChars));
                userOTPResponseViewModel.ExpiresAt = DateTime.Now.AddMinutes(10);
                userOTPResponseViewModel.Tries = 0;
                if (await StoreOTP(userOTPResponseViewModel, user))
                {
                    objReturn = this.SetResultStatus<UserOTPResponseViewModel>(userOTPResponseViewModel, "OTP sent successfully", true);
                    _smsService.SendSMS(user.PhoneNumber, $"{userOTPResponseViewModel.OTP} is your verification code for SmartPay");
                }
                else
                {
                    objReturn = this.SetResultStatus<UserOTPResponseViewModel>(userOTPResponseViewModel, "Fail to send OTP.", true);
                }
            }
            catch
            {
                objReturn = this.SetResultStatus<UserOTPResponseViewModel>(null, "Fail to send OTP.", false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<UserOTPVerifyResponseViewModel>> VerifyOTP(UserOTPVerifyRequestViewModel userOTPVerifyRequestViewModel)
        {
            ServiceResponse<UserOTPVerifyResponseViewModel> objReturn = new ServiceResponse<UserOTPVerifyResponseViewModel>();
            UserOTPVerifyResponseViewModel userOTPVerifyResponseViewModel = new UserOTPVerifyResponseViewModel();
            try
            {
                var user = await this._accountService.GetByEmailAsync(userOTPVerifyRequestViewModel.Email);
                if (user == null)
                {
                    this.SetResultStatus<UserOTPVerifyResponseViewModel>(null, MessageStatus.Error, false);
                }
                else
                {
                    userOTPVerifyResponseViewModel.MobileNo = user.PhoneNumber;

                    var userOTPRef = this._unitOfWork.UserOTPRepository.Find(x => x.UserId == user.Id).FirstOrDefault();

                    if ((userOTPRef.OTP == userOTPVerifyRequestViewModel.OTP && userOTPRef.ExpiresAt > DateTime.Now))
                    {
                        userOTPVerifyResponseViewModel.isOTPValid = true;
                        userOTPVerifyResponseViewModel.MobileNumberVerified = await confirmPhoneNumber(user);
                        objReturn = this.SetResultStatus<UserOTPVerifyResponseViewModel>(userOTPVerifyResponseViewModel, "Verified Successfully.", true);
                    }
                    else
                    {
                        userOTPVerifyResponseViewModel.isOTPValid = false;
                        objReturn = this.SetResultStatus<UserOTPVerifyResponseViewModel>(userOTPVerifyResponseViewModel, MessageStatus.Fail, true);
                    }
                }
            }
            catch
            {
                objReturn = this.SetResultStatus<UserOTPVerifyResponseViewModel>(null, MessageStatus.Fail, false);
            }

            return objReturn;
        }

        private async Task<bool> confirmPhoneNumber(DerivedIdentityUser user)
        {
            user.PhoneNumberConfirmed = true;
            user.PhoneNumberConfirmedOn = DateTime.UtcNow;
            if (await this._unitOfWork.UserAccountRepository.verifyPhoneNumber(user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
