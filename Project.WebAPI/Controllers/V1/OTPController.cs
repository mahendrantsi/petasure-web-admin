using SmartPay.Core.Extension;
using SmartPay.Models.CommonModel;
using SmartPay.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPay.WebAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : BaseController
    {
        private readonly IOTPService _otpService;

        public OTPController(IOTPService otpService)
        {
            this._otpService = otpService;
        }

        [HttpPost("SendOTP")]
        public async Task<IActionResult> SendOTPAsync([FromBody] UserOTPRequestViewModel userOTPReqViewModel)
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();
            try
            {
                if (ModelState.IsValid)
                {
                    var serviceResponse = await this._otpService.SendOTP(userOTPReqViewModel);
                    if (serviceResponse.IsSuccess)
                    {
                        response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Success, userMessage = serviceResponse.Message };
                        response.Data = serviceResponse.Data;
                    }
                    else
                    {
                        response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Fail, userMessage = serviceResponse.Message };
                        response.Data = null;
                    }
                }
                else
                {
                    response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Fail, userMessage = "Failed to sent OTP." };
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Fail, userMessage = "Failed to sent OTP." };
                response.Data = null;
            }

            return this.Ok(response);
        }

        [HttpPost("ResendOTP")]
        public async Task<IActionResult> ResendOTPAsync([FromBody] UserOTPRequestViewModel userOTPViewModel)
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();
            try
            {
                if (ModelState.IsValid)
                {
                    var serviceResponse = await this._otpService.ResendOTP(userOTPViewModel);
                    if (serviceResponse.IsSuccess)
                    {
                        response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Success, userMessage = serviceResponse.Message };
                        response.Data = serviceResponse.Data;
                    }
                    else
                    {
                        response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Success, userMessage = serviceResponse.Message };
                        response.Data = null;
                    }
                }
                else
                {
                    response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Success, userMessage = "Failed to sent OTP." };
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Success, userMessage = "Failed to sent OTP." };
                response.Data = null;
            }
            return this.Ok(response);
        }

        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTPAsync([FromBody] UserOTPVerifyRequestViewModel userOTPVerifyRequestViewModel)
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();
            try
            {
                if (ModelState.IsValid)
                {
                    var serviceResponse = await this._otpService.VerifyOTP(userOTPVerifyRequestViewModel);
                    if (serviceResponse.IsSuccess)
                    {
                        response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Success, userMessage = serviceResponse.Message };
                        response.Data = serviceResponse.Data;
                    }
                    else
                    {
                        response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Fail, userMessage = serviceResponse.Message };
                        response.Data = null;
                    }
                }
                else
                {
                    response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Fail, userMessage = "Failed to verify OTP." };
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Fail, userMessage = "Failed to verify OTP." };
                response.Data = null;
            }
            return this.Ok(response);
        }
    }
}
