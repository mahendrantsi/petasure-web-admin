using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Data.ExtendedDBEntities;
using Project.Models.AccountModel;
using Project.Models.Dashboard;
using Project.Services.IService;
using Project.Services.Service;
using Project.Services.ServiceEntities;
using Project.WebAPI.Infrastructure;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace Project.WebAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CheckUser")]
    public class DashboardController : BaseController
    { 
        private readonly IUserService userService; 

        public DashboardController( IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("User-Dashboard")] 
        public async Task<IActionResult> UserDashboard()
        {
            var serviceResponse = await this.userService.UserDashboard(base.GetCurrentUserId());
            return serviceResponse.IsSuccess ? this.Ok(serviceResponse.Data) : this.BadRequest(serviceResponse);
        }  
    }
}
