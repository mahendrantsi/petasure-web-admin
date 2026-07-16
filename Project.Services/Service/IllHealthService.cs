using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Project.Core.Enum;
using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Models.GeneralModel;
using Project.Persistence.UOW;
using Project.Services.IService;
using Project.Services.ServiceEntities;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.Services.Service
{
    public class IllHealthService : BaseService, IIllHealthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IConfiguration configuataion;
        private readonly ILogger<IllHealthService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IllHealthService(
            IUnitOfWork unitOfWork,
            IExceptionLoggerService exceptionLoggerService,
            IConfiguration configuration,
            ILogger<IllHealthService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _exceptionLoggerService = exceptionLoggerService;
            configuataion = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private string CurrentRequestId =>
            _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N");

        public async Task<ServiceResponse<IllHealthResponse>> AnalyzeAsync(IllHealthAnalyzeRequest request)
        {
            var requestId = CurrentRequestId;
            try
            {
                _logger.LogInformation("Illness request received: petId={PetId} species={Species} requestId={RequestId}", request?.PetId, request?.Species, requestId);

                if (request?.Image == null || !Guid.TryParse(request.PetId, out var petGuid))
                {
                    return this.SetResultStatus(IllHealthGuidanceMapper.Map(null), MessageStatus.Error, false);
                }

                var pet = _unitOfWork.PetRepository.GetPetDataByPetId(petGuid);
                if (pet == null || pet.PetOwnerId != request.CurrentUserId)
                {
                    return this.SetResultStatus(IllHealthGuidanceMapper.Map(null), MessageStatus.PetNotExists, false);
                }

                // Most recent previously stored ill-health image for this pet, if any.
                var previousEvent = _unitOfWork.Instance.HealthCheckEvents
                    .Where(e => e.PetId == petGuid)
                    .OrderByDescending(e => e.CreatedOn)
                    .FirstOrDefault();
                var previousImagePath = previousEvent?.ImageRef;

                var currentImagePath = await SaveUploadedImageAsync(request.Image);

                // On AI failure this returns null; the mapper's fallback still carries the
                // disclaimer and we persist the event so the next scan has a previous image.
                var aiResult = await CallAiServiceAsync(request.Image, previousImagePath, petGuid, request.Species);

                // Recognition gate HARD-blocked the scan (not-a-pet / wrong species / different
                // pet). Do NOT persist a HealthCheckEvent — nothing was analyzed — and return the
                // blocking reason + message so mobile shows the right screen. The saved image is
                // left on disk (harmless; it lets a future audit see what was rejected).
                if (aiResult != null && !string.IsNullOrWhiteSpace(aiResult.ValidationError))
                {
                    _logger.LogInformation(
                        "Illness scan blocked by recognition gate: reason={Reason} petId={PetId} requestId={RequestId}",
                        aiResult.ValidationError, request.PetId, requestId);
                    var blocked = new IllHealthResponse
                    {
                        ValidationError = aiResult.ValidationError,
                        Message = aiResult.Message,
                        DetectedSpecies = aiResult.DetectedSpecies,
                        GuidanceText = aiResult.Message,
                    };
                    return this.SetResultStatus(blocked, MessageStatus.Error, false);
                }

                var response = IllHealthGuidanceMapper.Map(aiResult);

                var healthCheckEvent = new HealthCheckEvent
                {
                    PetId = petGuid,
                    Species = request.Species,
                    ImageRef = currentImagePath,
                    PreviousImageRef = previousImagePath,
                    SubmittedAt = DateTime.UtcNow,
                    Status = EnumHealthCheckStatus.Pending,
                    AiSummary = aiResult?.Summary,
                    DisclaimerShown = true,
                    ModelVersion = aiResult?.ModelVersion,
                    CreatedBy = request.CurrentUserId,
                };

                // Link this illness event back to the in-process recognition-gate check that
                // ran alongside it (see illhealth_api.py's classify_pet call before the blur
                // gate). Setting the navigation property lets EF fix up PetScanId on save,
                // even though the PetScans row's Id is DB-generated and not yet known here.
                if (aiResult != null && !string.IsNullOrWhiteSpace(aiResult.DetectedSpecies))
                {
                    var detectedSpecies = aiResult.DetectedSpecies.Trim().ToLowerInvariant() switch
                    {
                        "dog" => EnumRecognitionSpecies.Dog,
                        "cat" => EnumRecognitionSpecies.Cat,
                        _ => EnumRecognitionSpecies.Unknown,
                    };

                    healthCheckEvent.PetScan = new PetScans
                    {
                        PetId = petGuid,
                        ScanType = EnumPetScanType.Classify,
                        Species = detectedSpecies,
                        ClassifierLabel = aiResult.DetectedSpecies,
                        RouteDecision = aiResult.SpeciesMismatch ? "mismatch" : "match",
                        Status = EnumPetScanStatus.Success,
                        Notes = aiResult.SpeciesMismatch
                            ? $"Recognition gate: claimed species '{request.Species}' did not match detected '{aiResult.DetectedSpecies}'."
                            : null,
                        CreatedBy = request.CurrentUserId,
                    };
                }

                foreach (var condition in response.Conditions)
                {
                    healthCheckEvent.HealthStatuses.Add(new HealthStatus
                    {
                        ConditionName = condition.ConditionName,
                        AffectedArea = condition.AffectedArea,
                        Confidence = condition.Confidence,
                        Severity = condition.Severity,
                        CreatedBy = request.CurrentUserId,
                    });
                }

                await _unitOfWork.Instance.HealthCheckEvents.AddAsync(healthCheckEvent);
                var saved = await _unitOfWork.SaveChangesAsync();
                if (!saved)
                {
                    // SaveChangesAsync() now logs the real exception itself (see UnitOfWork),
                    // but the caller must still check this — previously this return value was
                    // discarded entirely, so a failed save (e.g. the ConditionName NOT NULL
                    // violation this masked) was reported to mobile as a success with no row
                    // ever actually persisted.
                    _logger.LogWarning("Illness scan DB save failed (see UnitOfWork log above) requestId={RequestId}", requestId);
                    return this.SetResultStatus(IllHealthGuidanceMapper.Map(null), MessageStatus.Error, false);
                }

                _logger.LogInformation("Database saved: healthCheckEventId={EventId} requestId={RequestId}", healthCheckEvent.Id, requestId);

                response.EventId = healthCheckEvent.Id.ToString();

                _logger.LogInformation("Illness response returned: eventId={EventId} requestId={RequestId}", response.EventId, requestId);
                return this.SetResultStatus(response, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Illness analysis failed requestId={RequestId}", requestId);
                _exceptionLoggerService.LogException(ex);
                // Even on failure, the response still carries the disclaimer (default on IllHealthResponse).
                return this.SetResultStatus(IllHealthGuidanceMapper.Map(null), MessageStatus.Error, false);
            }
        }

        private async Task<string> SaveUploadedImageAsync(Microsoft.AspNetCore.Http.IFormFile image)
        {
            var webProjectRootPath = configuataion.GetValue<string>("WebProjectRootPath");
            var uploads = Path.Combine(webProjectRootPath, "uploads", "illhealth");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var fileExtension = Path.GetExtension(image.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploads, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            _logger.LogInformation("Image stored: path={FileName} sizeBytes={SizeBytes} requestId={RequestId}", fileName, image.Length, CurrentRequestId);

            return Path.Combine("/uploads", "illhealth", fileName).Replace("\\", "/");
        }

        /// <summary>
        /// Forwards the current (and, when available, previous) image to the Python AI service.
        /// Contract: multipart/form-data with file parts current_image (required) + previous_image
        /// (optional, both image/jpeg) and form fields pet_id + species. Returns null on any
        /// transport/timeout/non-2xx failure so the caller can fall back gracefully (never throws).
        /// </summary>
        private async Task<IllHealthAiResult> CallAiServiceAsync(
            Microsoft.AspNetCore.Http.IFormFile currentImage,
            string previousImagePath,
            Guid petId,
            EnumHealthCheckSpecies species)
        {
            try
            {
                var aiBaseUrl = configuataion["PythonAiService:IllHealthAiUrl"];
                var aiApiKey = configuataion["PythonAiService:IllHealthAiApiKey"];

                using var form = new MultipartFormDataContent();

                using (var ms = new MemoryStream())
                {
                    await currentImage.CopyToAsync(ms);
                    var fileContent = new ByteArrayContent(ms.ToArray());
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    form.Add(fileContent, "current_image", currentImage.FileName);
                }

                if (!string.IsNullOrWhiteSpace(previousImagePath))
                {
                    var webProjectRootPath = configuataion.GetValue<string>("WebProjectRootPath");
                    var previousFullPath = Path.Combine(webProjectRootPath, previousImagePath.TrimStart('/', '\\'));
                    if (File.Exists(previousFullPath))
                    {
                        var previousBytes = await File.ReadAllBytesAsync(previousFullPath);
                        var previousContent = new ByteArrayContent(previousBytes);
                        previousContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        form.Add(previousContent, "previous_image", Path.GetFileName(previousFullPath));
                    }
                }

                // Form fields expected alongside the image parts.
                form.Add(new StringContent(petId.ToString()), "pet_id");
                form.Add(new StringContent(species.ToString().ToLowerInvariant()), "species");

                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(aiBaseUrl),
                    Timeout = TimeSpan.FromSeconds(30),
                };
                if (!string.IsNullOrWhiteSpace(aiApiKey))
                {
                    httpClient.DefaultRequestHeaders.Add("X-API-Key", aiApiKey);
                }
                var requestId = CurrentRequestId;
                httpClient.DefaultRequestHeaders.Add("X-Request-Id", requestId);

                _logger.LogInformation("Python request: endpoint=ai/illhealth/analyze petId={PetId} species={Species} requestId={RequestId}", petId, species, requestId);

                var response = await httpClient.PostAsync("ai/illhealth/analyze", form);
                _logger.LogInformation("Python response: statusCode={StatusCode} requestId={RequestId}", (int)response.StatusCode, requestId);

                // 422 = a deliberate recognition-gate BLOCK (not-a-pet / wrong species /
                // different pet). Its body carries validation_error + message, which we must
                // parse and propagate — so do NOT EnsureSuccessStatusCode() it away. Any other
                // non-2xx (AI down, 500, 401) returns null for the graceful fallback below.
                if (!response.IsSuccessStatusCode && (int)response.StatusCode != 422)
                {
                    _logger.LogWarning("Python illness AI returned {StatusCode} requestId={RequestId}", (int)response.StatusCode, requestId);
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IllHealthAiResult>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            }
            catch (Exception ex)
            {
                // AI down / timeout / non-2xx / bad payload must NOT bubble up as a 500 or a hard
                // failure. Log and return null so AnalyzeAsync maps a clean fallback result.
                _logger.LogWarning(ex, "Python illness AI call failed requestId={RequestId}", CurrentRequestId);
                _exceptionLoggerService.LogException(ex);
                return null;
            }
        }
    }
}
