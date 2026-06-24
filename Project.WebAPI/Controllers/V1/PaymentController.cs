using SmartPay.Core.Extension;
using SmartPay.Services.IService;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("GetToken")]
        public async Task<IActionResult> GetToken()
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();
            var UserId = GetCurrentUserId();
            var serviceResponse = await _paymentService.GenerateToken(UserId);
            if (serviceResponse.IsSuccess)
            {
                response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Success, userMessage = serviceResponse.Message };
                response.Data = serviceResponse.Data;
            }
            return this.Ok(response);
        }
    }
}
