using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Project.Core.Enum;
using Project.Models.CommonModel;
using Project.Models.Master;
using Project.Services.IService;
using Project.Services.Service;
using Project.Web.Common;
using Project.Web.Resources;
using Project.Web.UiUtility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [CustomAuthorize(Roles = "Admin")]
    public class SettingsController : BaseController
    {
        private readonly IToastNotification toastNotification;
        private readonly ISettingService _settingService;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        public SettingsController(IToastNotification objToastNotification, ISettingService settingService, 
            IExceptionLoggerService exceptionLoggerService)
        {
            this.toastNotification = objToastNotification;
            this._settingService = settingService;
            this._exceptionLoggerService = exceptionLoggerService;
        }

        public async Task<IActionResult> FAQ()
        {
            var serviceResponse = await _settingService.GetFAQ();
            return View(serviceResponse.Data);
        }
        [HttpPost]
        public async Task<ActionResult> CreateFAQ(FAQViewModel model)
        {
            model.CreatedBy = base.GetCurrentUserId();
            if (model.Id is not null || model.Id == Guid.Empty)
            {
                return Json(await _settingService.UpdateFAQ(model));
            }
            else
            {
                return Json(await _settingService.InsertFAQ(model));
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetFaq(string id)
        {
            var serviceResponse = await _settingService.GetFAQbyID(Guid.Parse(id));
            return Json(serviceResponse);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFaq(string id)
        {
            var serviceResponse = await _settingService.DeleteFAQ(Guid.Parse(id));
            return Json(serviceResponse);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFAQOrder(List<FaqOrder> request)
        {
            var result = await _settingService.UpdateAllFaqOrder(request);
            return Json(result);
        }
    }
}
