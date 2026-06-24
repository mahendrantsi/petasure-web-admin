using Microsoft.AspNetCore.Mvc;
using Project.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Persistence.UOW;
using Project.Services.IService;

namespace Project.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [CustomAuthorize(Roles = "Admin,SubAdmin,AnonymousUser")]
    public class DashBoardController : BaseController
    {
        private readonly IUserService _userService;
        public DashBoardController(IUserService userService)
        {
            _userService = userService; 
        }
        public async Task<IActionResult> Index()
        {
            @ViewData["Title"] = "Dashboard";
            var serviceResponse = await this._userService.GetAdminDashboard();
            return View(serviceResponse.Data);
        }
    }
}
