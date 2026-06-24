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
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string keyWord)
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();
            try
            {
                var serviceResponse = await this._userService.SearchUser(Convert.ToInt64(GetCurrentUserId()), keyWord);
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
            catch (Exception ex)
            {
                response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Fail, userMessage = "Failed" };
                response.Data = null;
            }

            return this.Ok(response);
        }

        [HttpGet("Recents")]
        public async Task<IActionResult> Recents(int number)
        {
            DateTime reqTime = DateTime.UtcNow;
            var response = new ResponseData();
            try
            {
                var serviceResponse = await this._userService.RecentUserList(Convert.ToInt64(GetCurrentUserId()));
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
            catch (Exception ex)
            {
                response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Fail, userMessage = "Failed" };
                response.Data = null;
            }

            return this.Ok(response);
        }
    }
}
