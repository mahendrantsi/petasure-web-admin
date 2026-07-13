using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Enum;
using Project.Models.GeneralModel;
using Project.Services.IService;
using System.Threading.Tasks;

namespace Project.WebAPI.Controllers.V1
{
    /// <summary>
    /// Ill-health / early issue detection endpoints (T28). Every response from
    /// <see cref="IIllHealthService.AnalyzeAsync"/> carries the non-diagnostic disclaimer
    /// (Doc 2) by construction, so no path here can skip it.
    /// </summary>
    [Route("api/illhealth")]
    [ApiController]
    [Authorize(Policy = "CheckUser")]
    public class IllHealthController : BaseController
    {
        private readonly IIllHealthService _illHealthService;

        public IllHealthController(IIllHealthService illHealthService)
        {
            _illHealthService = illHealthService;
        }

        [HttpPost("dog")]
        public async Task<IActionResult> Dog([FromForm] IllHealthUploadModel model)
        {
            var response = await _illHealthService.AnalyzeAsync(new IllHealthAnalyzeRequest
            {
                PetId = model.PetId,
                Image = model.Image,
                Species = EnumHealthCheckSpecies.Dog,
                CurrentUserId = base.GetCurrentUserId(),
            });

            return response.IsSuccess ? this.Ok(response.Data) : this.BadRequest(response.Data);
        }

        [HttpPost("cat")]
        public async Task<IActionResult> Cat([FromForm] IllHealthUploadModel model)
        {
            var response = await _illHealthService.AnalyzeAsync(new IllHealthAnalyzeRequest
            {
                PetId = model.PetId,
                Image = model.Image,
                Species = EnumHealthCheckSpecies.Cat,
                CurrentUserId = base.GetCurrentUserId(),
            });

            return response.IsSuccess ? this.Ok(response.Data) : this.BadRequest(response.Data);
        }
    }

    public class IllHealthUploadModel
    {
        public string PetId { get; set; }
        public IFormFile Image { get; set; }
    }
}
