using Microsoft.AspNetCore.Mvc;
using Project.Services.IService;
using Project.Services.Service;
using Project.Web.Common;

namespace Project.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [CustomAuthorize(Roles = "Admin,SubAdmin")]
    public class MissingPetsController : BaseController
    {
        private readonly IMissingService missingService;
        public MissingPetsController(IMissingService _missingService)
        {
            missingService = _missingService;
        }
        public IActionResult Index()
        {
            var response = missingService.AdminMissingPetInfos().GetAwaiter().GetResult();
            return View(response.Data);
        }
        public IActionResult Cats()
        {
            var response = missingService.AdminMissingPetInfos().GetAwaiter().GetResult();
            return View(response.Data);
        }
    }
}