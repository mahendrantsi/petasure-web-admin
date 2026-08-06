using Microsoft.AspNetCore.Mvc;
using Project.Models.AdminModel;
using Project.Services.IService;
using Project.Web.Common;
using System;
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
        public async Task<IActionResult> Index(string status = null, string search = null, int page = 1)
        {
            ViewData["Title"] = "Alert Centre";
            ViewBag.Search = search;
            var filter = new AlertFilterViewModel { Status = status, Search = search, PageNumber = page };
            var serviceResponse = await _alertCentreService.GetAlerts(filter);

            if (serviceResponse.IsSuccess)
            {
                // Ensure ActiveStatus is set so the dropdown binds correctly
                serviceResponse.Data.ActiveStatus = string.IsNullOrEmpty(status) ? "All Status" : status;
                return View(serviceResponse.Data);
            }

            return View(new AlertCentreViewModel { ActiveStatus = string.IsNullOrEmpty(status) ? "All Status" : status });
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["Title"] = "Alert Details";
            var serviceResponse = await _alertCentreService.GetAlertDetail(id);

            if (serviceResponse.IsSuccess && serviceResponse.Data != null)
            {
                return View(serviceResponse.Data);
            }

            TempData["Error"] = serviceResponse.Message ?? "Alert not found.";
            return RedirectToAction("Index");
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
                var newAlertsCount = serviceResponse.Data.Statistics.NewAlerts;
                return Json(new { count = newAlertsCount });
            }

            return Json(new { count = 0 });
        }
    }
}
