using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Pets;
using Project.Services.IService;
using Project.Web.Common;

namespace Project.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [CustomAuthorize(Roles = "AnonymousUser")]
    public class AnonymousUserController : BaseController
    {
        private readonly IPetService petService;
        private readonly IAccountService accountService;
        public AnonymousUserController(IPetService _petService, IAccountService _accountService)
        {
            petService = _petService;
            accountService = _accountService;
        }

        public IActionResult Index(string? search)
        {
            var response = petService.AdminPetInfos().GetAwaiter().GetResult();
            var pets = response.Data;

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                pets = pets.Where(p =>
                    (!string.IsNullOrEmpty(p.MicrochipNumber) && p.MicrochipNumber.Contains(search))
                ).ToList();
                ViewBag.Search = search;
            }
            else {
                pets = new List<PetsViewModel>();
            }


            return View(pets.OrderByDescending(a => a.CreatedOn).ToList());
        }
    }
}
