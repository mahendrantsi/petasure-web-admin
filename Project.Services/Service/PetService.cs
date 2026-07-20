using Project.Data.DBEntities;
using Project.Services.IService;
using Project.Services.Resources;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models.Pets;
using Project.Core.Extension;
using Project.Models.Master;
using Project.Core.Enum;
using Project.Data.ExtendedDBEntities;
using Project.Models.AccountModel;
using Project.Persistence.UOW;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using Project.Models.GeneralModel;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Text.Json;
using ServiceStack;
using static System.Net.Mime.MediaTypeNames;


namespace Project.Services.Service
{

    public class PetService : BaseService, IPetService
    {
        //UnitOfWork Accessing for DB Tables
        private readonly IUnitOfWork _unitOfWork;

        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IConfiguration configuataion;
        private readonly ILogger<PetService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PetService(
            IUnitOfWork unitOfWork,
            IExceptionLoggerService exceptionLoggerService,
            IConfiguration configuration,
            ILogger<PetService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _exceptionLoggerService = exceptionLoggerService;
            configuataion = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        // Correlates one recognition scan's log lines across .NET and (via the X-Request-Id
        // header sent to Python) the AI service's own logs. Falls back to a fresh id when
        // called outside an HTTP request (e.g. a background job).
        private string CurrentRequestId =>
            _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N");


        /// <summary>
        /// Get Pet Listing according to currentUser Buisness Logic
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<List<PetsViewModel>>> petInfos(Guid userID)
        {
            var response = new ServiceResponse<List<PetsViewModel>>();
            try
            {
                var petInfoList = _unitOfWork.PetRepository.GetPetList(userID).OrderByDescending(a => a.CreatedOn).ToList();

                if (petInfoList != null)
                {
                    response = this.SetResultStatus<List<PetsViewModel>>(petInfoList, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<List<PetsViewModel>>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<List<PetsViewModel>>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Get All Pet Listing for ADMIN Buisness Logic
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<List<PetsViewModel>>> AdminPetInfos()
        {
            var response = new ServiceResponse<List<PetsViewModel>>();
            try
            {
                var petInfoList = _unitOfWork.PetRepository.GetPetAll();

                if (petInfoList != null)
                {
                    response = this.SetResultStatus<List<PetsViewModel>>(petInfoList, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<List<PetsViewModel>>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<List<PetsViewModel>>(null, Messages_Resources.Error, false);
            }
            return response;
        }


        /// <summary>
        /// Create New Pet Buisness Logic
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> CreateNewPet(PetsViewModel petData)
        {
            ServiceResponse<PetsViewModel> objReturn;
            try
            {
                //Save Data in Repo
                var responseId = await _unitOfWork.PetRepository.SavePetDataAsync(petData);

                if (!string.IsNullOrEmpty(responseId))
                {
                    petData.Id = Guid.Parse(responseId);
                    objReturn = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.PetAdded, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<PetsViewModel>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<PetsViewModel>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }


        /// <summary>
        /// Create New Pet Buisness Logic
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> UpdatePet(PetsViewModel petData)
        {
            ServiceResponse<PetsViewModel> objReturn;
            try
            {
                //Check Existing Pet Data
                PetsViewModel existingTempPet = _unitOfWork.PetRepository.GetPetDataByPetId(petData.Id);

                if (existingTempPet is null)
                    return this.SetResultStatus<PetsViewModel>(null, MessageStatus.PetNotExists, false);

                //Save Data in Repo
                var success = await _unitOfWork.PetRepository.UpdatePetDataAsync(petData);

                if (success)
                {
                    objReturn = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.PetUpdated, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<PetsViewModel>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<PetsViewModel>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }


        //Delete Pet Buisness Logic
        public async Task<ServiceResponse<string>> DeletePet(Guid petId)
        {
            ServiceResponse<string> response;
            try
            {
                var isSuccess = await _unitOfWork.PetRepository.DeletePetAsync(petId);

                if (isSuccess)
                {
                    DeletePetsOnAI(new List<Guid> { petId });
                    response = this.SetResultStatus<string>("Success", MessageStatus.PetDeleted, true);

                }
                else
                {
                    response = this.SetResultStatus<string>(null, MessageStatus.PetDeleteError, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Pet Detail By GUID pet Id
        /// </summary>
        /// <param name="petId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> petDetail(Guid petId)
        {
            var response = new ServiceResponse<PetsViewModel>();
            try
            {
                var petData = _unitOfWork.PetRepository.GetPetData(petId);

                if (petData != null)
                {
                    response = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Pet Detail by String Pet Id For ADMIN
        /// </summary>
        /// <param name="petId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> petDetail(string petId, string baseURL)
        {
            var response = new ServiceResponse<PetsViewModel>();
            try
            {
                var petData = _unitOfWork.PetRepository.GetPetDataByStringId(petId, baseURL);

                if (petData != null)
                {
                    response = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Pet Detail By microCip Number
        /// </summary>
        /// <param name="microNumber"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> petDetailByMicroNumber(String microNumber)
        {
            var response = new ServiceResponse<PetsViewModel>();
            try
            {
                var petData = _unitOfWork.PetRepository.GetPetData(microNumber);

                if (petData != null)
                {
                    response = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Delete All Pets of User on Cancel or Expire Subscription
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productTitle"></param>
        /// <returns></returns>
        public bool DeleteAllPets(Guid userId, string productTitle)
        {
            if (!string.IsNullOrEmpty(productTitle))
            {
                var petAddCount = 0;

                if (productTitle.ToLower().Contains("multi pet"))
                {
                    petAddCount = 5;
                }
                else if (productTitle.ToLower().Contains("two pet"))
                {
                    petAddCount = 2;
                }
                else
                {
                    petAddCount = 1;
                }

                var allPets = _unitOfWork.PetRepository.GetAll().Where(a => a.UserID == userId).OrderByDescending(o => o.CreatedOn).Take(petAddCount);
                if (allPets.Any())
                {
                    var petIds = allPets.Select(s => s.Id).ToList();
                    var allMissingPets = _unitOfWork.MissingPetRepository.GetAll().Where(a => a.PetId != null && petIds.Contains(a.PetId.Value));
                    if (allMissingPets.Any())
                        _unitOfWork.MissingPetRepository.RemoveRange(allMissingPets);
                    _unitOfWork.PetRepository.RemoveRange(allPets);

                    //delete all the pets on AI
                    DeletePetsOnAI(allPets.Select(s => s.Id).ToList()).GetAwaiter().GetResult();
                    return true;
                }
            }
            else //delete all pets if product title will be null
            {
                var allPets = _unitOfWork.PetRepository.GetAll().Where(a => a.UserID == userId);
                if (allPets.Any())
                {
                    var petIds = allPets.Select(s => s.Id).ToList();
                    var allMissingPets = _unitOfWork.MissingPetRepository.GetAll().Where(a => a.PetId != null && petIds.Contains(a.PetId.Value));
                    if (allMissingPets.Any())
                        _unitOfWork.MissingPetRepository.RemoveRange(allMissingPets);
                    _unitOfWork.PetRepository.RemoveRange(allPets);

                    //delete all the pets on AI
                    DeletePetsOnAI(allPets.Select(s => s.Id).ToList()).GetAwaiter().GetResult();
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // ===== RECOGNITION PERSISTENCE HELPERS =======================
        // ============================================================
        // Every recognition call below saves its image(s) to disk + a PetImages row,
        // forwards to the Python AI service, parses whatever it can out of the response
        // into a PetScans row, and records a RecognitionErrors row on any failure — all
        // under a single SaveChangesAsync() per call. Preserves the existing wire contract
        // (still returns the raw AI response string) so no mobile-side change is required
        // for this step.

        private async Task<PetImages> SaveImageAsync(Microsoft.AspNetCore.Http.IFormFile file, EnumImageKind kind, Guid? petId)
        {
            var webProjectRootPath = configuataion.GetValue<string>("WebProjectRootPath");
            var uploadsDir = Path.Combine(webProjectRootPath, "uploads", "recognition");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadsDir, fileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            _logger.LogInformation(
                "Image stored: path={StoragePath} sizeBytes={SizeBytes} requestId={RequestId}",
                fileName, file.Length, CurrentRequestId);

            return new PetImages
            {
                PetId = petId,
                ImageKind = kind,
                StoragePath = Path.Combine("/uploads", "recognition", fileName).Replace("\\", "/"),
                OriginalFileName = file.FileName,
                ContentType = file.ContentType,
                FileSizeBytes = file.Length,
            };
        }

        private static byte[] ReadAllBytes(Microsoft.AspNetCore.Http.IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Defensively pulls whatever fields it recognizes out of the AI service's response
        /// body (shape: {success, status, message, data:{...}}) into the PetScans columns.
        /// Unknown/missing fields are left null rather than failing the scan.
        /// </summary>
        private static void ApplyAiResponseToScan(PetScans scan, HttpStatusCode statusCode, string responseContent)
        {
            scan.AiStatusCode = (int)statusCode;
            scan.AiResponseRaw = string.IsNullOrEmpty(responseContent) || responseContent.Length <= 4000
                ? responseContent
                : responseContent.Substring(0, 4000);

            try
            {
                using var doc = JsonDocument.Parse(responseContent);
                var root = doc.RootElement;
                var data = root.TryGetProperty("data", out var d) && d.ValueKind == JsonValueKind.Object ? d : root;

                if (data.TryGetProperty("dog_exist", out var dogExist))
                {
                    scan.MatchResult = dogExist.ValueKind == JsonValueKind.True ? "matched"
                        : dogExist.ValueKind == JsonValueKind.False ? "no_match" : scan.MatchResult;
                }
                if (data.TryGetProperty("distance", out var distance) && distance.ValueKind == JsonValueKind.Number && distance.TryGetDecimal(out var distanceVal))
                {
                    scan.MatchConfidence = distanceVal;
                }
                if (data.TryGetProperty("ds_id", out var dsId) && dsId.ValueKind == JsonValueKind.String)
                {
                    scan.MatchedDsId = dsId.GetString();
                }
                if (data.TryGetProperty("is_blur", out var isBlur))
                {
                    scan.IsBlurRejected = isBlur.ValueKind == JsonValueKind.True;
                }
                if (data.TryGetProperty("nose_detect", out var noseDetect))
                {
                    scan.IsNoseDetected = noseDetect.ValueKind == JsonValueKind.True ? true
                        : noseDetect.ValueKind == JsonValueKind.False ? false : scan.IsNoseDetected;
                }
                if (data.TryGetProperty("label", out var label) && label.ValueKind == JsonValueKind.String)
                {
                    scan.ClassifierLabel = label.GetString();
                }
                if (data.TryGetProperty("route", out var route) && route.ValueKind == JsonValueKind.String)
                {
                    scan.RouteDecision = route.GetString();
                }
                if (data.TryGetProperty("confidence", out var confidence) && confidence.ValueKind == JsonValueKind.Number && confidence.TryGetDecimal(out var confVal))
                {
                    scan.ClassifierConfidence = confVal;
                }
                if (data.TryGetProperty("dog_score", out var dogScore) && dogScore.ValueKind == JsonValueKind.Number && dogScore.TryGetDecimal(out var dogScoreVal))
                {
                    scan.ClassifierDogScore = dogScoreVal;
                }
                if (data.TryGetProperty("cat_score", out var catScore) && catScore.ValueKind == JsonValueKind.Number && catScore.TryGetDecimal(out var catScoreVal))
                {
                    scan.ClassifierCatScore = catScoreVal;
                }
            }
            catch (JsonException)
            {
                // Non-JSON or unexpected shape — AiResponseRaw is kept for troubleshooting;
                // leave the structured columns null rather than fail the whole scan.
            }

            scan.Status = (int)statusCode >= 200 && (int)statusCode < 300
                ? (scan.IsBlurRejected || scan.RouteDecision == "reject" || scan.MatchResult == "no_match"
                    ? EnumPetScanStatus.Rejected
                    : EnumPetScanStatus.Success)
                : EnumPetScanStatus.Failed;
        }

        // 400 (not 201): this is a malformed request from the caller, not an AI-driven
        // not-a-pet/wrong-species verdict — RecognitionScanResult (BaseController) will
        // propagate this status verbatim so it's visibly distinct from a validation popup.
        private static string InvalidPetIdResponse()
        {
            return "{\"success\":false,\"status\":400,\"message\":\"A valid PetId is required to register biometrics for this pet.\"}";
        }

        private async Task<string> FailImageSaveAsync(PetScans scan, Exception ex)
        {
            scan.Status = EnumPetScanStatus.Failed;
            scan.Notes = ex.Message;
            await _unitOfWork.Instance.RecognitionErrors.AddAsync(new RecognitionErrors
            {
                PetScan = scan,
                ErrorStage = EnumRecognitionErrorStage.ImageSave,
                ErrorMessage = ex.Message,
            });
            _exceptionLoggerService.LogException(ex);

            try
            {
                await _unitOfWork.Instance.PetScans.AddAsync(scan);
                var saved = await _unitOfWork.SaveChangesAsync();
                if (!saved)
                {
                    // SaveChangesAsync() logs the real exception itself; this just pinpoints
                    // which caller's save failed (see UnitOfWork.SaveChangesAsync for why
                    // checking this return value matters — it used to be silently discarded
                    // everywhere, which is exactly what hid the illness ConditionName bug).
                    _logger.LogWarning("Failed to persist failed-image-save PetScans row for scan {ScanType}", scan.ScanType);
                }
            }
            catch (Exception dbEx)
            {
                _exceptionLoggerService.LogException(dbEx);
            }

            return "{\"success\":false,\"status\":500,\"message\":\"Could not save uploaded image, please try again.\"}";
        }

        private async Task<string> ExecuteRecognitionScanAsync(PetScans scan, string endpoint, Action<MultipartFormDataContent> buildForm)
        {
            var requestId = CurrentRequestId;
            var stage = EnumRecognitionErrorStage.AiRequest;
            string responseContent = null;
            try
            {
                var reqUrl = configuataion["CustomKeys:DogRequestUrl"];
                var reqKey = configuataion["CustomKeys:DogRequestApiKey"];

                using var form = new MultipartFormDataContent();
                buildForm(form);

                var httpClient = new HttpClient { BaseAddress = new Uri(reqUrl) };
                if (!string.IsNullOrWhiteSpace(reqKey))
                {
                    httpClient.DefaultRequestHeaders.Add("X-API-Key", reqKey);
                }
                httpClient.DefaultRequestHeaders.Add("X-Request-Id", requestId);

                _logger.LogInformation(
                    "Python request: endpoint={Endpoint} scanType={ScanType} species={Species} requestId={RequestId}",
                    endpoint, scan.ScanType, scan.Species, requestId);

                var stopwatch = Stopwatch.StartNew();
                var response = await httpClient.PostAsync(endpoint, form);
                stopwatch.Stop();
                scan.AiRequestDurationMs = (int)stopwatch.ElapsedMilliseconds;

                stage = EnumRecognitionErrorStage.AiResponseParse;
                responseContent = await response.Content.ReadAsStringAsync();
                ApplyAiResponseToScan(scan, response.StatusCode, responseContent);

                _logger.LogInformation(
                    "Python response: endpoint={Endpoint} statusCode={StatusCode} durationMs={DurationMs} requestId={RequestId}",
                    endpoint, (int)response.StatusCode, scan.AiRequestDurationMs, requestId);
            }
            catch (Exception ex)
            {
                scan.Status = EnumPetScanStatus.Failed;
                scan.Notes = ex.Message;
                await _unitOfWork.Instance.RecognitionErrors.AddAsync(new RecognitionErrors
                {
                    PetScan = scan,
                    ErrorStage = stage,
                    ErrorMessage = ex.Message,
                });
                _logger.LogWarning(ex, "Recognition scan failed at stage={Stage} endpoint={Endpoint} requestId={RequestId}", stage, endpoint, requestId);
                _exceptionLoggerService.LogException(ex);
                responseContent ??= "{\"success\":false,\"status\":500,\"message\":\"Recognition service unavailable, please try again.\"}";
            }

            try
            {
                await _unitOfWork.Instance.PetScans.AddAsync(scan);
                var saved = await _unitOfWork.SaveChangesAsync();
                if (saved)
                {
                    _logger.LogInformation("Database saved: scanId={ScanId} status={Status} requestId={RequestId}", scan.Id, scan.Status, requestId);
                }
                else
                {
                    // SaveChangesAsync() logs the real exception itself (see UnitOfWork) —
                    // this just pinpoints which caller's save failed.
                    _logger.LogWarning("Failed to persist PetScans row requestId={RequestId}", requestId);
                }
            }
            catch (Exception dbEx)
            {
                _logger.LogWarning(dbEx, "Failed to persist PetScans row requestId={RequestId}", requestId);
                _exceptionLoggerService.LogException(dbEx);
            }

            _logger.LogInformation("Response returned: endpoint={Endpoint} requestId={RequestId}", endpoint, requestId);
            return responseContent;
        }

        /// <summary>
        /// Check Similar Dog in the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> SimilarDogRequest(SimilarDogRequestViewModel model)
        {
            var scan = new PetScans { ScanType = EnumPetScanType.Similar, Species = EnumRecognitionSpecies.Dog };
            try
            {
                scan.PrimaryImage = await SaveImageAsync(model.Image, EnumImageKind.NoseImage, null);
            }
            catch (Exception ex)
            {
                return await FailImageSaveAsync(scan, ex);
            }

            return await ExecuteRecognitionScanAsync(scan, "similar", form =>
            {
                var fileContent = new ByteArrayContent(ReadAllBytes(model.Image));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                // here it is important that second parameter matches with name given in API.
                form.Add(fileContent, "nose_image", model.Image.FileName);
                // User-selected species so Python can hard-reject a wrong-species / not-a-pet photo.
                form.AddParam("species", string.IsNullOrWhiteSpace(model.Species) ? "dog" : model.Species);
            });
        }

        /// <summary>
        /// Analyze Dog Full Image for Breed and other details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> AnalyzeDogRequest(AnalyzeDogRequestViewModel model)
        {
            var scan = new PetScans { ScanType = EnumPetScanType.Analyze, Species = EnumRecognitionSpecies.Dog };
            try
            {
                scan.PrimaryImage = await SaveImageAsync(model.NoseImage, EnumImageKind.NoseImage, null);
                scan.SecondaryImage = await SaveImageAsync(model.DogImage, EnumImageKind.FullBodyImage, null);
            }
            catch (Exception ex)
            {
                return await FailImageSaveAsync(scan, ex);
            }

            return await ExecuteRecognitionScanAsync(scan, "analyze", form =>
            {
                var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                var noseExt = Path.GetExtension(model.NoseImage.FileName);
                var noseContent = new ByteArrayContent(ReadAllBytes(model.NoseImage));
                noseContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(noseContent, "nose_image", $"{timestamp}_nose_image{noseExt}");

                var dogExt = Path.GetExtension(model.DogImage.FileName);
                var dogContent = new ByteArrayContent(ReadAllBytes(model.DogImage));
                dogContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                // here it is important that second parameter matches with name given in API.
                form.Add(dogContent, "dog_image", $"{timestamp}_dog_image{dogExt}");
                form.AddParam("species", string.IsNullOrWhiteSpace(model.Species) ? "dog" : model.Species);
            });
        }

        /// <summary>
        /// Register Dog to the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> RegisterDogRequest(RegisterDogRequestViewModel model)
        {
            // A registration scan with no valid PetId is worse than useless: it still saves images,
            // still calls the AI, and still sends "ds_id" to the AI's permanent embedding store below
            // (form.AddParam("ds_id", model.PetId)) — with an invalid/missing id that can never be
            // traced back to a real pet, and a PetScans row that can't be linked to one either. Reject
            // up front instead of silently completing with an orphaned scan + a garbage embedding.
            if (!Guid.TryParse(model.PetId, out var petId))
            {
                _logger.LogWarning("Pet/Register rejected: missing or invalid PetId (received: {PetId})", model.PetId);
                return InvalidPetIdResponse();
            }
            var scan = new PetScans { ScanType = EnumPetScanType.Register, Species = EnumRecognitionSpecies.Dog, PetId = petId };
            try
            {
                scan.PrimaryImage = await SaveImageAsync(model.NoseImage, EnumImageKind.NoseImage, petId);
                scan.SecondaryImage = await SaveImageAsync(model.DogImage, EnumImageKind.FullBodyImage, petId);
            }
            catch (Exception ex)
            {
                return await FailImageSaveAsync(scan, ex);
            }

            return await ExecuteRecognitionScanAsync(scan, "register", form =>
            {
                var noseContent = new ByteArrayContent(ReadAllBytes(model.NoseImage));
                noseContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(noseContent, "nose_image", model.PetId + "_nose_image" + Path.GetExtension(model.NoseImage.FileName));

                var dogContent = new ByteArrayContent(ReadAllBytes(model.DogImage));
                dogContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                // here it is important that second parameter matches with name given in API.
                form.Add(dogContent, "dog_image", model.PetId + "_dog_image" + Path.GetExtension(model.DogImage.FileName));

                form.AddParam("ds_id", model.PetId);
                form.AddParam("species", string.IsNullOrWhiteSpace(model.Species) ? "dog" : model.Species);
            });
        }

        // Cats use the same nose-based AI service as dogs; the service auto-classifies the
        // species, so we send the same field names ("nose_image"/"dog_image") and hit the
        // same endpoint/URL as the dog flow.
        public async Task<string> SimilarCatRequest(SimilarCatRequestViewModel model)
        {
            var scan = new PetScans { ScanType = EnumPetScanType.Similar, Species = EnumRecognitionSpecies.Cat };
            try
            {
                scan.PrimaryImage = await SaveImageAsync(model.Image, EnumImageKind.NoseImage, null);
            }
            catch (Exception ex)
            {
                return await FailImageSaveAsync(scan, ex);
            }

            return await ExecuteRecognitionScanAsync(scan, "similar", form =>
            {
                var fileContent = new ByteArrayContent(ReadAllBytes(model.Image));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "nose_image", model.Image.FileName);
                form.AddParam("species", string.IsNullOrWhiteSpace(model.Species) ? "cat" : model.Species);
            });
        }

        public async Task<string> AnalyzeCatRequest(AnalyzeCatRequestViewModel model)
        {
            var scan = new PetScans { ScanType = EnumPetScanType.Analyze, Species = EnumRecognitionSpecies.Cat };
            try
            {
                scan.PrimaryImage = await SaveImageAsync(model.NoseImage, EnumImageKind.NoseImage, null);
                scan.SecondaryImage = await SaveImageAsync(model.CatImage, EnumImageKind.FullBodyImage, null);
            }
            catch (Exception ex)
            {
                return await FailImageSaveAsync(scan, ex);
            }

            return await ExecuteRecognitionScanAsync(scan, "analyze", form =>
            {
                var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                var noseExt = Path.GetExtension(model.NoseImage.FileName);
                var noseContent = new ByteArrayContent(ReadAllBytes(model.NoseImage));
                noseContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(noseContent, "nose_image", $"{timestamp}_nose_image{noseExt}");

                var catExt = Path.GetExtension(model.CatImage.FileName);
                var catContent = new ByteArrayContent(ReadAllBytes(model.CatImage));
                catContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(catContent, "dog_image", $"{timestamp}_dog_image{catExt}");
                form.AddParam("species", string.IsNullOrWhiteSpace(model.Species) ? "cat" : model.Species);
            });
        }

        public async Task<string> RegisterCatRequest(RegisterCatRequestViewModel model)
        {
            // See RegisterDogRequest's identical guard: an invalid/missing PetId here would still
            // reach the AI's permanent embedding store via "ds_id" below and still write an
            // unlinkable PetScans row — reject up front instead.
            if (!Guid.TryParse(model.PetId, out var petId))
            {
                _logger.LogWarning("Pet/RegisterCat rejected: missing or invalid PetId (received: {PetId})", model.PetId);
                return InvalidPetIdResponse();
            }
            var scan = new PetScans { ScanType = EnumPetScanType.Register, Species = EnumRecognitionSpecies.Cat, PetId = petId };
            try
            {
                scan.PrimaryImage = await SaveImageAsync(model.NoseImage, EnumImageKind.NoseImage, petId);
                scan.SecondaryImage = await SaveImageAsync(model.CatImage, EnumImageKind.FullBodyImage, petId);
            }
            catch (Exception ex)
            {
                return await FailImageSaveAsync(scan, ex);
            }

            return await ExecuteRecognitionScanAsync(scan, "register", form =>
            {
                var noseContent = new ByteArrayContent(ReadAllBytes(model.NoseImage));
                noseContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(noseContent, "nose_image", model.PetId + "_nose_image" + Path.GetExtension(model.NoseImage.FileName));

                var catContent = new ByteArrayContent(ReadAllBytes(model.CatImage));
                catContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(catContent, "dog_image", model.PetId + "_dog_image" + Path.GetExtension(model.CatImage.FileName));

                form.AddParam("ds_id", model.PetId);
                form.AddParam("species", string.IsNullOrWhiteSpace(model.Species) ? "cat" : model.Species);
            });
        }

        //delete the pets on AI
        public async Task<string> DeletePetsOnAI(List<Guid> petIds)
        {
            var dogReqUrl = configuataion["CustomKeys:DogRequestUrl"];
            var dogReqKey = configuataion["CustomKeys:DogRequestApiKey"];

            string commaSeparatedIds = string.Join(",", petIds.Select(x => x));

            using var form = new MultipartFormDataContent();
            form.AddParam("ds_ids", commaSeparatedIds);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(dogReqUrl)
            };
            httpClient.DefaultRequestHeaders.Add("X-API-Key", dogReqKey);
            var response = await httpClient.PostAsync($"delete_multiple", form);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        
    }
}
