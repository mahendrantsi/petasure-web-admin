using Microsoft.AspNetCore.Mvc;
using Project.Models.Master;
using Project.Services.IService;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Web.Areas.Admin.Controllers
{
    public class IntegrationManagementController : BaseController
    {
        private readonly IIntegrationService _integrationService;
        public IntegrationManagementController(IIntegrationService integrationService) 
        {
            _integrationService = integrationService;
        }
        public async Task<IActionResult> Index()
        {
            var integrationList = _integrationService.Get();
            return View(integrationList.Data);
        }

        [HttpGet]
        public IActionResult Integration() 
        {
            return View(new IntegrationViewModel());
        }
        [HttpPost]
        public IActionResult Integration(IntegrationViewModel model)
        {

            return View();
        }
    }
}
