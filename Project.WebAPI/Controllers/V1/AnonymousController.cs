using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project;
using Project.Data.DBEntities;
using Project.Models.GeneralModel;
using Project.Models.Pets;
using Project.Services.IService;
using Project.Services.Service;
using Project.WebAPI;
using Project.WebAPI.Controllers;
using Project.WebAPI.Controllers.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.WebAPI.Controllers.V1
{
    /// <summary>
    /// API endpoints for anonymous actions such as reporting a found pet and basic pet lookups.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnonymousController : BaseController
    {
        private readonly IMissingService _missingService;
        private readonly IPetService _petService;
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        /// <summary>
        /// Initializes a new instance of <see cref="AnonymousController"/>.
        /// </summary>
        /// <param name="missingService">Service for missing pet business logic.</param>
        /// <param name="accountService">Account service used to create/check anonymous ID users.</param>
        /// <param name="petService">Pet service for pet lookups.</param>
        /// <param name="emailService">Email service for notifications.</param>
        /// <param name="exceptionLoggerService">Logger used for non-critical/secondary failures.</param>
        public AnonymousController(IMissingService missingService, IAccountService accountService, IPetService petService, IEmailService emailService, IExceptionLoggerService exceptionLoggerService)
        {

            _missingService = missingService;
            _accountService = accountService;
            _petService = petService;
            _emailService = emailService;
            _exceptionLoggerService = exceptionLoggerService;
        }

        /// <summary>
        /// Register a found missing pet reported by an anonymous guest or for ID check workflow.
        /// </summary>
        /// <param name="model">Details about the found pet and reporter information.</param>
        /// <returns>200 OK with service response on success; 400 BadRequest for validation or failures.</returns>
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

                // Check Email and Contact Number
                // Add Anonymous user in DB
                if (model.Email != null && model.ContactNumber != null)
                {
                    var accountResponse = await _accountService.CreateIDCheckUser(model);

                    if (accountResponse.Data != null)
                    {
                        // Temporary workaround: the anonymous UserID from CreateIDCheckUser is expected
                        // to already exist in AspNetUsers, but we defensively re-verify it here so a
                        // never-persisted/invalid Guid becomes null instead of tripping
                        // FK_MissingPets_AspNetUsers_FoundBy inside the pet-found update below.
                        model.FoundBy = await this.ResolveFoundByAsync(accountResponse.Data.UserID);

                        // Primary operation: this must succeed/fail on its own merits.
                        var response = await _missingService.FoundMissingPetByAnonymous(model);
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
                    else
                    {
                        return BadRequest(new { Message = "User Not Registered" });
                    }

                }

                return BadRequest(new { Message = "Please provide contact details" });
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

        /// <summary>
        /// Returns pet details by GUID.
        /// </summary>
        /// <param name="petId">Pet identifier GUID.</param>
        /// <returns>200 OK with pet data when found; 400 on error.</returns>
        [HttpGet("PetDetail")]
        public async Task<IActionResult> GetPetDetail(Guid petId)
        {

            var response = await _petService.petDetail(petId);

            return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);

        }

        /// <summary>
        /// Returns pet detail by microchip number.
        /// </summary>
        /// <param name="microChipNumber">Microchip identifier string.</param>
        /// <returns>200 OK with pet data when found; 400 on error.</returns>
        [HttpGet("PetDetailByMicroChipNumber")]
        public async Task<IActionResult> FoundPetByMicroChipNumber(String microChipNumber)
        {

            var response = await _petService.petDetailByMicroNumber(microChipNumber);

            return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);

        }


        /// <summary>
        /// Proxy endpoint to find similar dogs by image (anonymous).
        /// </summary>
        /// <param name="model">Image payload for similarity check.</param>
        /// <returns>200 OK with service response; 201 when the AI hard-rejects the photo; 400 on error.</returns>
        [AllowAnonymous]
        [HttpPost("Similar")]
        public async Task<IActionResult> Similar(SimilarDogRequestViewModel model)
        {
            try
            {
                var response = await _petService.SimilarDogRequest(model);
                return this.RecognitionScanResult(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        /// <summary>
        /// Proxy endpoint to find similar cats by image (anonymous).
        /// </summary>
        /// <param name="model">Image payload for similarity check.</param>
        /// <returns>200 OK with service response; 201 when the AI hard-rejects the photo; 400 on error.</returns>
        [AllowAnonymous]
        [HttpPost("SimilarCat")]
        public async Task<IActionResult> SimilarCat(SimilarCatRequestViewModel model)
        {
            try
            {
                var response = await _petService.SimilarCatRequest(model);
                return this.RecognitionScanResult(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        /// <summary>
        /// Analyze dog images for breed and metadata (anonymous).
        /// </summary>
        /// <param name="model">Images required for analysis.</param>
        /// <returns>200 OK with the analysis response; 201 when the AI hard-rejects the photo; 400 on error.</returns>
        [AllowAnonymous]
        [HttpPost("AnalyzeDog")]
        public async Task<IActionResult> AnalysImage(AnalyzeDogRequestViewModel model)
        {
            try
            {
                var response = await _petService.AnalyzeDogRequest(model);
                return this.RecognitionScanResult(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        /// <summary>
        /// Analyze cat images for breed and metadata (anonymous).
        /// </summary>
        /// <param name="model">Images required for analysis.</param>
        /// <returns>200 OK with the analysis response; 201 when the AI hard-rejects the photo; 400 on error.</returns>
        [AllowAnonymous]
        [HttpPost("AnalyzeCat")]
        public async Task<IActionResult> AnalyzeCat(AnalyzeCatRequestViewModel model)
        {
            try
            {
                var response = await _petService.AnalyzeCatRequest(model);
                return this.RecognitionScanResult(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }
    }
}
