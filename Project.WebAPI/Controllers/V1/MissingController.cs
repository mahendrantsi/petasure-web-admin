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
        private readonly IExceptionLoggerService _exceptionLoggerService;


        //Constructor for Pet Controller
        public MissingController(IMissingService missingService, IAccountService accountService, IPetService petService, IEmailService emailService, IExceptionLoggerService exceptionLoggerService)
        {
            _missingService = missingService;
            _accountService = accountService;
            _petService = petService;
            _emailService = emailService;
            _exceptionLoggerService = exceptionLoggerService;
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

                // Temporary workaround: never trust a client-supplied FoundBy. Always prefer the
                // authenticated caller's own id, and fall back to null (rather than an invalid
                // Guid) if it doesn't resolve to an existing AspNetUsers record, so the value is
                // guaranteed safe before it ever reaches SaveChangesAsync and can never trip
                // FK_MissingPets_AspNetUsers_FoundBy.
                model.FoundBy = await this.ResolveFoundByAsync(this.GetCurrentUserId());

                // Primary operation: this must succeed/fail on its own merits.
                var response = await _missingService.FoundMissingPet(model);
                if (response.IsSuccess)
                {
                    var petData = await _petService.petDetail(model.PetId);
                    if (petData.IsSuccess && petData.Data != null)
                    {
                        // Secondary/non-critical operation: a failure here must not affect
                        // the already-successful pet-found update above.
                        try
                        {
                            await _emailService.SendFoundMissingPetSupportEmail(model.Email, petData.Data.PName, model.ContactNumber);
                        }
                        catch (Exception emailEx)
                        {
                            await _exceptionLoggerService.LogException(emailEx);
                        }
                    }
                    return this.Ok(response);
                }
                return this.BadRequest(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        /// <summary>
        /// Resolves a candidate FoundBy id to a value safe to persist: the id itself when it
        /// exists in AspNetUsers, otherwise null. Prevents FK_MissingPets_AspNetUsers_FoundBy
        /// from ever reaching SaveChangesAsync with an invalid value.
        /// </summary>
        private async Task<Guid?> ResolveFoundByAsync(Guid candidateId)
        {
            if (candidateId == Guid.Empty)
            {
                return null;
            }

            var userCheck = await _accountService.GetUserDetailById(candidateId);
            return userCheck.IsSuccess && userCheck.Data != null ? candidateId : (Guid?)null;
        }
    }
}
