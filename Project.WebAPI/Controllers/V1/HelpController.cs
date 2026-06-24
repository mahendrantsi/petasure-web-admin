using SmartPay.Core.Extension;
using SmartPay.Models.CommonModel;
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
    public class HelpController : BaseController
    {
        private readonly IHelpService _helpservice;

        public HelpController(IHelpService helpService)
        {
            _helpservice = helpService;
        }

        [HttpPost("ContactUs")]
        [AllowAnonymous]
        public async Task<ActionResult> ContactUs(EnquiryViewModel model)
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();

            if (ModelState.IsValid)
            {
                var serviceResponse = await _helpservice.SaveUserMessage(model);
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

            return this.Ok(response);
        }

        [HttpGet("getContent")]
        [AllowAnonymous]
        public async Task<ActionResult> getContent(string ContentType)
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();

            var serviceResponse = await _helpservice.getContent(ContentType);
            if (serviceResponse.IsSuccess)
            {
                response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Success,  userMessage = serviceResponse.Message };
                response.Data = serviceResponse.Data;
            }
            else
            {
                response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Fail,  userMessage = serviceResponse.Message };
                response.Data = null;
            }

            return this.Ok(response);
        }
    }
}
