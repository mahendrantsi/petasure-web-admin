using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CheckUser")]
    public class PetController : BaseController
    {
       
        private readonly IPetService _petService;
        private readonly ILogger<PetController> _logger;


        /// <summary>
        /// Constructor for Pet Controller
        /// </summary>
        /// <param name="petService"></param>
        public PetController( IPetService petService, ILogger<PetController> logger)
        {

            _petService = petService;
            _logger = logger;
        }


        /// <summary>
        /// Get Pet Info for current Login User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPetInfo(Guid userId)
        {

            var response = await _petService.petInfos(userId);

            return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);
            
        }

        /// <summary>
        /// Add Pet Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost] //("AddPetInfo")
        public async Task<IActionResult> AddPetInfo(PetsViewModel model)
        {
            try {
                 model.PetOwnerId = base.GetCurrentUserId();
                 var response = await _petService.CreateNewPet(model);
                 return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePetInfo(PetsViewModel model)
        {
            try
            {
                var response = await _petService.UpdatePet(model);
                return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        /// <summary>
        /// Get Pet Info for current Login User
        /// </summary>
        /// <param name="petId"></param>
        /// <returns></returns>
        [HttpGet("DeletePet")]
        public async Task<IActionResult> DeletePet(Guid petId)
        { 
            var response = await _petService.DeletePet(petId);
            return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);
        }

        

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDogRequestViewModel model)
        {
            _logger.LogInformation("Request received: Pet/Register petId={PetId} traceId={TraceId}", model?.PetId, HttpContext.TraceIdentifier);
            try
            {
                var response = await _petService.RegisterDogRequest(model);
                _logger.LogInformation("Response returned: Pet/Register traceId={TraceId}", HttpContext.TraceIdentifier);
                return this.Ok(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        [HttpPost("RegisterCat")]
        public async Task<IActionResult> RegisterCat(RegisterCatRequestViewModel model)
        {
            _logger.LogInformation("Request received: Pet/RegisterCat petId={PetId} traceId={TraceId}", model?.PetId, HttpContext.TraceIdentifier);
            try
            {
                var response = await _petService.RegisterCatRequest(model);
                _logger.LogInformation("Response returned: Pet/RegisterCat traceId={TraceId}", HttpContext.TraceIdentifier);
                return this.Ok(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }
    }
}
