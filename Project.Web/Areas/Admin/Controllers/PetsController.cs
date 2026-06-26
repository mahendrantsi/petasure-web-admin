using Microsoft.AspNetCore.Mvc;
using Project.Models.CommonModel;
using Project.Models.Pets;
using Project.Services.IService;
using Project.Services.Service;
using Project.Web.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [CustomAuthorize(Roles = "Admin,SubAdmin,AnonymousUser")]
    public class PetsController : BaseController
    {
        private readonly IPetService petService;
        private readonly IAccountService accountService;
        public PetsController(IPetService _petService, IAccountService _accountService)
        {
            petService = _petService;
            accountService = _accountService;
        }

        public IActionResult Index(string? search)
        {
            var response = petService.AdminPetInfos().GetAwaiter().GetResult();
            var pets = response.Data ?? new List<PetsViewModel>();

            var hasTypeData = pets.Any(p => p.PetTypeId.HasValue);
            if (hasTypeData)
            {
                pets = pets.Where(p => p.PetTypeId == 1).ToList();
            }
            else
            {
                ViewBag.TypeWarning = "No pet type data available; showing all pets.";
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                pets = pets.Where(p =>
                    (!string.IsNullOrEmpty(p.PName) && p.PName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.PSex) && p.PSex.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.MicrochipNumber) && p.MicrochipNumber.Contains(search)) ||
                    (!string.IsNullOrEmpty(p.ContactNumber) && p.ContactNumber.ToLower().Contains(search))
                ).ToList();
                ViewBag.Search = search;
            }

            return View(pets.OrderByDescending(a => a.CreatedOn).ToList());
        }

        public IActionResult Cats(string? search)
        {
            var response = petService.AdminPetInfos().GetAwaiter().GetResult();
            var pets = response.Data ?? new List<PetsViewModel>();
            var hasTypeData = pets.Any(p => p.PetTypeId.HasValue);
            if (hasTypeData)
            {
                pets = pets.Where(p => p.PetTypeId == 2).ToList();
            }
            else
            {
                ViewBag.TypeWarning = "No pet type data available; showing all pets.";
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                pets = pets.Where(p =>
                    (!string.IsNullOrEmpty(p.PName) && p.PName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.PSex) && p.PSex.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.MicrochipNumber) && p.MicrochipNumber.Contains(search)) ||
                    (!string.IsNullOrEmpty(p.ContactNumber) && p.ContactNumber.ToLower().Contains(search))
                ).ToList();
                ViewBag.Search = search;
            }

            return View(pets.OrderByDescending(a => a.CreatedOn).ToList());
        }


        [HttpGet]
        [CustomAuthorize(Roles = "Admin,SubAdmin,AnonymousUser")]
        public async Task<IActionResult> PetDetail(string petId)
        {

            var petData = await petService.petDetail(petId, Project.Web.Common.ConfigurationManager.GetBaseUrl());
            if (petData.Data != null)
            {
                return View(petData.Data);
            }
            return View(new PetsViewModel());
        }

        [HttpGet("download")]
        public IActionResult DownloadUserList(string? search, int? petTypeId)
        {
            var response = petService.AdminPetInfos().GetAwaiter().GetResult();
            var petList = response.Data ?? new List<PetsViewModel>();

            if (petTypeId.HasValue)
            {
                petList = petList.Where(p => p.PetTypeId.HasValue && p.PetTypeId == petTypeId.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                petList = petList.Where(p =>
                    (!string.IsNullOrEmpty(p.PName) && p.PName.Contains(search)) ||
                    (!string.IsNullOrEmpty(p.PSex) && p.PSex.Contains(search)) ||
                    (!string.IsNullOrEmpty(p.MicrochipNumber) && p.MicrochipNumber.Contains(search)) ||
                    (!string.IsNullOrEmpty(p.ContactNumber) && p.ContactNumber.Contains(search))
                ).ToList();
            }

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Pet Name,Sex,Address,Contact Number,Owner Name,Is Missing,Created On,Microchip Number,Breeder,Licence Number,Issuing Authority,Breed Description,Colour,Date Of Birth");

            foreach (var pet in petList.OrderByDescending(a => a.CreatedOn))
            {
                var userDetail = accountService.GetUserDetailById(pet.PetOwnerId).GetAwaiter().GetResult();
                var userName = "";
                if (userDetail != null && userDetail.IsSuccess)
                {
                    userName = userDetail.Data.FirstName + " " + userDetail.Data.LastName;
                }

                csvBuilder.AppendLine(
                    $"{Escape(pet.PName)}," +
                    $"{Escape(pet.PSex)}," +
                    $"{Escape(pet.Address)}," +
                    $"{pet.ContactNumber}," +
                    $"{userName}," +
                    $"{pet.IsMissing}," +
                    $"{pet.CreatedOn:yyyy-MM-dd HH:mm:ss}," +
                    $"{Escape(pet.MicrochipNumber)}," +
                    $"{Escape(pet.Breeder)}," +
                    $"{Escape(pet.LicenceNumber)}," +
                    $"{Escape(pet.IssuingAuthority)}," +
                    $"{Escape(pet.BreedDescription)}," +
                    $"{Escape(pet.Colour)}," +
                    $"{(pet.DateOfBirth.HasValue ? pet.DateOfBirth.Value.ToString("yyyy-MM-dd") : "")}"
                );
            }

            byte[] buffer = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return File(buffer, "text/csv", "PetList.csv");
        }


        // Helper to safely handle commas and newlines in CSV
        private static string Escape(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return $"\"{input.Replace("\"", "\"\"")}\"";
        }

    }
}
