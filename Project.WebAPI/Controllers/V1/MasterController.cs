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
    
    public class MasterController : BaseController
    {
        private readonly IMasterService _masterService;
        public MasterController(IMasterService masterService)
        {
            _masterService = masterService;
        }

        [HttpGet("getCategories")]
        public async Task<IActionResult> getCategories()
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();

            if (ModelState.IsValid)
            {
                var serviceResponse = await _masterService.getCategories();
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

        [HttpGet("getCurrencies")]
        public async Task<IActionResult> getCurrencies()
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();

            if (ModelState.IsValid)
            {
                var serviceResponse = await _masterService.getCurrencies();
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
    }
}
