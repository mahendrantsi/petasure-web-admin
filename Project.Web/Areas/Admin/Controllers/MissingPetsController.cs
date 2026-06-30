using Microsoft.AspNetCore.Mvc;
using Project.Services.IService;
using Project.Services.Service;
using Project.Web.Common;
using System.Collections.Generic;
using Project.Models.Pets;
using System.Linq;

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
        public IActionResult Dogs()
        {
            var response = missingService.AdminMissingPetInfos().GetAwaiter().GetResult();
            var list = response.Data ?? new List<MissingPetsViewModel>();
            var hasTypeData = list.Any(p => p.PetTypeId.HasValue);
            if (hasTypeData)
            {
                list = list.Where(p => p.PetTypeId == 1).ToList();
            }
            return View(list);
        }
        public IActionResult Cats()
        {
            var response = missingService.AdminMissingPetInfos().GetAwaiter().GetResult();
            var list = response.Data ?? new List<MissingPetsViewModel>();
            var hasTypeData = list.Any(p => p.PetTypeId.HasValue);
            if (hasTypeData)
            {
                list = list.Where(p => p.PetTypeId == 2).ToList();
            }
            return View(list);
        }
    }
}