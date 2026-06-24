using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.Data.DBEntities;
using Project.Models.Pets;
using Project.Services.IService;
using Project.Services.Service;
using Project.Services.ServiceEntities;
using System;
using System.Threading.Tasks;

namespace Project.WebAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CheckUser")]
    public class MissingController : BaseController
    {
        private readonly IMissingService _missingService;
        private readonly IAccountService _accountService;
        private readonly IPetService _petService;
        private readonly IEmailService _emailService;


        //Constructor for Pet Controller
        public MissingController(IMissingService missingService, IAccountService accountService, IPetService petService, IEmailService emailService)
        {
            _missingService = missingService;
            _accountService = accountService;
            _petService = petService;
            _emailService = emailService;
        }

        [HttpPost("ReportMissingPet")]
        public async Task<IActionResult> ReportMissingPet(MissingPetRequestViewModel model)
        {
            try
            {
                var response = await _missingService.ReportMissingPet(model, GetCurrentUserName());
                return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        /// <summary>
        /// Used for ID Check process as well as Found pet 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("FoundMyPet")]
        public async Task<IActionResult> FoundMyPet(FoundMissingPetRequest model)
        {
            try
            {
                // Validate input
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model), "Bad Request, Data cannot be null");
                }

                var response = await _missingService.FoundMyPet(model);
                return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        /// <summary>
        /// Used for ID Check process as well as Found pet 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("FoundMissingPet")]
        public async Task<IActionResult> FoundMissingPet(FoundMissingPetRequest model)
        {
            try
            {
                // Validate input
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model), "Bad Request, Data cannot be null");
                }
                var response = await _missingService.FoundMissingPet(model);
                if (response.IsSuccess)
                {
                    var petData = await _petService.petDetail(model.PetId);
                    await _emailService.SendFoundMissingPetSupportEmail(model.Email, petData.Data.PName, model.ContactNumber);
                    return this.Ok(response);
                }
                return this.BadRequest(response);

                return BadRequest(new { Message = "Please provide contact details" });
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }
    }
}
