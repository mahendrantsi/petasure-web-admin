using Project.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Project.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Authorization;
using Project.Web.Common;

namespace Project.Web.Areas.Merchant.Controllers
{
    [Area("User")]
	[CustomAuthorize(Roles = "User,Merchant")]
	public class DashboardController : BaseController
    {
        private readonly IUserService _userService;
        public DashboardController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            @ViewData["Title"] = "Dashboard";
            var serviceResponse = await this._userService.GetUserDashboardDetails(base.GetCurrentUserId());
            return View(serviceResponse.Data);
        }
    }
}
