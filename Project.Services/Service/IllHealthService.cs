using Microsoft.Extensions.Configuration;
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

        public IllHealthService(IUnitOfWork unitOfWork, IExceptionLoggerService exceptionLoggerService)
        {
            _unitOfWork = unitOfWork;
            _exceptionLoggerService = exceptionLoggerService;
            configuataion = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
        }

        public async Task<ServiceResponse<IllHealthResponse>> AnalyzeAsync(IllHealthAnalyzeRequest request)
        {
            try
            {
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

                var aiResult = await CallAiServiceAsync(request.Image, previousImagePath);

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
                await _unitOfWork.SaveChangesAsync();

                response.EventId = healthCheckEvent.Id.ToString();

                return this.SetResultStatus(response, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
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

            return Path.Combine("/uploads", "illhealth", fileName).Replace("\\", "/");
        }

        private async Task<IllHealthAiResult> CallAiServiceAsync(Microsoft.AspNetCore.Http.IFormFile currentImage, string previousImagePath)
        {
            var aiBaseUrl = configuataion["PythonAiService:IllHealthAiUrl"];
            var aiApiKey = configuataion["PythonAiService:IllHealthAiApiKey"];

            using var form = new MultipartFormDataContent();

            using (var ms = new MemoryStream())
            {
                await currentImage.CopyToAsync(ms);
                var fileContent = new ByteArrayContent(ms.ToArray());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
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
                    previousContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    form.Add(previousContent, "previous_image", Path.GetFileName(previousFullPath));
                }
            }

            var httpClient = new HttpClient { BaseAddress = new Uri(aiBaseUrl) };
            if (!string.IsNullOrWhiteSpace(aiApiKey))
            {
                httpClient.DefaultRequestHeaders.Add("X-API-Key", aiApiKey);
            }

            var response = await httpClient.PostAsync("ai/illhealth/analyze", form);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IllHealthAiResult>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }
    }
}
