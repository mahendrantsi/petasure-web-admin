using Microsoft.AspNetCore.Mvc;
using Project.Models.AdminModel;
using Project.Services.IService;
using Project.Web.Common;
using System.Threading.Tasks;

namespace Project.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [CustomAuthorize(Roles = "Admin,SubAdmin")]
    public class AlertCentreController : BaseController
    {
        private readonly IAlertCentreService _alertCentreService;

        public AlertCentreController(IAlertCentreService alertCentreService)
        {
            _alertCentreService = alertCentreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string status = null)
        {
            ViewData["Title"] = "Alert Centre";
            var serviceResponse = await _alertCentreService.GetAlertsByPage(page, 10, status);

            if (serviceResponse.IsSuccess)
            {
                return View(serviceResponse.Data);
            }

            return View(new AlertCentreViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Filter(AlertFilterViewModel filter)
        {
            var serviceResponse = await _alertCentreService.GetAlerts(filter);

            if (serviceResponse.IsSuccess)
            {
                return Json(new { success = true, data = serviceResponse.Data });
            }

            return Json(new { success = false, message = serviceResponse.Message });
        }

        [HttpGet]
        public async Task<IActionResult> GetNewAlertsCount()
        {
            var serviceResponse = await _alertCentreService.GetAlerts();

            if (serviceResponse.IsSuccess)
            {
                var totalAlertsCount = serviceResponse.Data.Statistics.TotalAlerts;
                return Json(new { count = totalAlertsCount });
            }

            return Json(new { count = 0 });
        }
    }
}
