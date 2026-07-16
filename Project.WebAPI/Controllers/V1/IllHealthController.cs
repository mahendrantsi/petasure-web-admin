using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<IllHealthController> _logger;

        public IllHealthController(IIllHealthService illHealthService, ILogger<IllHealthController> logger)
        {
            _illHealthService = illHealthService;
            _logger = logger;
        }

        [HttpPost("dog")]
        public async Task<IActionResult> Dog([FromForm] IllHealthUploadModel model)
        {
            _logger.LogInformation("Request received: illhealth/dog petId={PetId} traceId={TraceId}", model?.PetId, HttpContext.TraceIdentifier);

            var response = await _illHealthService.AnalyzeAsync(new IllHealthAnalyzeRequest
            {
                PetId = model.PetId,
                Image = model.Image,
                Species = EnumHealthCheckSpecies.Dog,
                CurrentUserId = base.GetCurrentUserId(),
            });

            _logger.LogInformation("Response returned: illhealth/dog success={Success} traceId={TraceId}", response.IsSuccess, HttpContext.TraceIdentifier);
            return response.IsSuccess ? this.Ok(response.Data) : this.BadRequest(response.Data);
        }

        [HttpPost("cat")]
        public async Task<IActionResult> Cat([FromForm] IllHealthUploadModel model)
        {
            _logger.LogInformation("Request received: illhealth/cat petId={PetId} traceId={TraceId}", model?.PetId, HttpContext.TraceIdentifier);

            var response = await _illHealthService.AnalyzeAsync(new IllHealthAnalyzeRequest
            {
                PetId = model.PetId,
                Image = model.Image,
                Species = EnumHealthCheckSpecies.Cat,
                CurrentUserId = base.GetCurrentUserId(),
            });

            _logger.LogInformation("Response returned: illhealth/cat success={Success} traceId={TraceId}", response.IsSuccess, HttpContext.TraceIdentifier);
            return response.IsSuccess ? this.Ok(response.Data) : this.BadRequest(response.Data);
        }
    }

    public class IllHealthUploadModel
    {
        public string PetId { get; set; }
        public IFormFile Image { get; set; }
    }
}
