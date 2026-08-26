using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Core.Enum;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.AdminModel;
using Project.Models.CommonModel;
using Project.Models.Dashboard;
using Project.Persistence.UOW;
using Project.Services.IService;
using Project.Services.Resources;
using Project.Services.ServiceEntities;
using ServiceStack.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _imageBaseUrl;
        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            // Recognition/illness images are saved and served by Project.WebAPI, a
            // separate host from this admin dashboard app — a bare relative path like
            // "/uploads/..." resolves against THIS app's own origin in the browser and
            // 404s. Prepend the API's public base URL, matching the existing convention
            // in PetRepository.cs (NoseImagePath = baseURL + petData.NoseImagePath).
            _imageBaseUrl = (configuration["CustomKeys:BaseUrl"] ?? string.Empty).TrimEnd('/');
        }

        private string ResolveImageUrl(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return relativePath;
            if (relativePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                relativePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return relativePath;
            }
            return _imageBaseUrl + "/" + relativePath.TrimStart('~', '/');
        }

        // A /similar or /analyze scan never carries PetId (see PetScans.PetId doc comment) —
        // only /register does. On a match, the only clue to which pet it is is MatchedDsId,
        // which registration sends to the AI as the pet's own Guid (PetService.RegisterDogRequest /
        // RegisterCatRequest: form.AddParam("ds_id", model.PetId)). So the effective pet id for a
        // scan is PetId when present, else MatchedDsId parsed as a Guid.
        private static Guid? GetEffectivePetId(Guid? petId, string matchedDsId)
        {
            if (petId.HasValue) return petId;
            return Guid.TryParse(matchedDsId, out var parsed) ? parsed : (Guid?)null;
        }

        // Guid.TryParse can't be translated into SQL, so this resolves names against
        // already-materialized scan rows rather than inside the EF query.
        private async Task<Dictionary<Guid, PetInfo>> GetPetInfoLookupAsync(IEnumerable<(Guid? PetId, string MatchedDsId)> scans)
        {
            var petIds = scans
                .Select(s => GetEffectivePetId(s.PetId, s.MatchedDsId))
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .Distinct()
                .ToList();

            if (petIds.Count == 0) return new Dictionary<Guid, PetInfo>();

            return await _unitOfWork.Instance.PetInfo
                .Where(p => petIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);
        }
        public Task<ServiceResponse<UserProfile>> DeleteProfile(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<DashboardViewModel>> GetAdminDashboard(int page = 1)
        {
            var response = new ServiceResponse<DashboardViewModel>();
            try
            {
                const int pageSize = 10;
                var currentPage = page < 1 ? 1 : page;

                var totalUser = _unitOfWork.UserAccountRepository.GetTotalUsers();
                var monthlyNewUsers = _unitOfWork.UserAccountRepository.GetMonthlyNewUsers();

                var lstMonthlyNewUsers = _unitOfWork.Instance.Users
                    .Select(u => u.CreatedOn)
                    .ToList()
                    .GroupBy(d => new { d.Year, d.Month })
                    .Select(g => new MonthlyUsers
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        UserCount = g.Count()
                    })
                    .OrderBy(result => result.Year).ThenBy(result => result.Month)
                    .ToList();

                // Get count of cats and dogs using PetTypeId (1 = Dog, 2 = Cat) directly from DB
                var numberOfDogs = _unitOfWork.Instance.PetInfo.Count(p => p.PetTypeId == 1);
                var numberOfCats = _unitOfWork.Instance.PetInfo.Count(p => p.PetTypeId == 2);

                // ── Scan metrics — based directly on MatchResult column ──────────────────
                // Summary cards count only rows where MatchResult is explicitly set.
                // "matched" → Matched count   |   "no_match" → Unmatched count
                var allScans = await _unitOfWork.Instance.PetScans
                    .Select(s => new { s.MatchResult })
                    .ToListAsync();

                var matchedScans = allScans.Count(s => s.MatchResult != null &&
                    (s.MatchResult.ToLower() == "matched" || s.MatchResult.ToLower() == "match" || s.MatchResult.ToLower() == "possible_match"));
                var unmatchedScans = allScans.Count(s => s.MatchResult != null &&
                    (s.MatchResult.ToLower() == "no_match" || s.MatchResult.ToLower() == "unmatched" || s.MatchResult.ToLower() == "unmatch"));
                var totalScans = allScans.Count;

                // ── Error breakdown by stage ────────────────────────────────────────────
                var allErrors = await _unitOfWork.Instance.RecognitionErrors
                    .Select(e => e.ErrorStage)
                    .ToListAsync();

                var errorTotal = allErrors.Count;
                var errorScans = errorTotal;
                var errorBreakdownItems = new List<ErrorBreakdownItem>
                {
                    BuildErrorItem("Image Quality",    allErrors, EnumRecognitionErrorStage.ImageSave,       errorTotal),
                    BuildErrorItem("No Face Detected", allErrors, EnumRecognitionErrorStage.RecognitionGate, errorTotal),
                    BuildErrorItem("AI Request Error", allErrors, EnumRecognitionErrorStage.AiRequest,       errorTotal),
                    BuildErrorItem("Response Parse",   allErrors, EnumRecognitionErrorStage.AiResponseParse, errorTotal),
                    BuildErrorItem("System Error",     allErrors, EnumRecognitionErrorStage.DbSave,          errorTotal),
                };

                // ── Pet Scan Logs (Dashboard) — only "matched" rows, paginated ─────────
                var petScanLogsQuery = _unitOfWork.Instance.PetScans
                    .Where(s => s.MatchResult != null &&
                                (s.MatchResult.ToLower() == "matched" ||
                                 s.MatchResult.ToLower() == "match" ||
                                 s.MatchResult.ToLower() == "possible_match"))
                    .OrderByDescending(s => s.CreatedOn);

                var totalScanLogs = await petScanLogsQuery.CountAsync();
                var petScanLogs = await petScanLogsQuery
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new
                    {
                        s.Id,
                        s.Species,
                        s.MatchResult,
                        s.RouteDecision,
                        s.Status,
                        s.MatchConfidence,
                        s.ClassifierConfidence,
                        s.CreatedOn,
                        s.ScanType,
                        s.Notes,
                        s.PetId,
                        s.MatchedDsId,
                    })
                    .ToListAsync();

                var petInfoLookup = await GetPetInfoLookupAsync(
                    petScanLogs.Select(s => (s.PetId, s.MatchedDsId)));

                var petScanLogViewModels = petScanLogs.Select(s =>
                {
                    var effectivePetId = GetEffectivePetId(s.PetId, s.MatchedDsId);
                    var pet = effectivePetId.HasValue && petInfoLookup.TryGetValue(effectivePetId.Value, out var p) ? p : null;

                    return new PetScanLogViewModel
                    {
                        Id = s.Id,
                        PetName = pet?.PName ?? "Unknown",
                        PetType = s.Species.ToString(),
                        // Use the pet's FullBodyImagePath from petinfo table (not the scan image)
                        PetImagePath = !string.IsNullOrWhiteSpace(pet?.FullBodyImagePath)
                            ? ResolveImageUrl(pet.FullBodyImagePath)
                            : "/images/pet-placeholder.svg",
                        Result = s.MatchResult,
                        Confidence = (s.MatchConfidence ?? s.ClassifierConfidence ?? 0m) * 100m,
                        ScanDate = s.CreatedOn,
                        ScanType = s.ScanType.ToString(),
                        Notes = s.Notes,
                    };
                }).ToList();

                // ── Match rate across all scan types ───────────────────────────────────
                var matchRate = totalScans > 0
                    ? (decimal)matchedScans / totalScans * 100m
                    : 0m;

                var dashboardDetails = new DashboardViewModel
                {
                    TotalUsers = totalUser,
                    MonthlyNewUsers = monthlyNewUsers,
                    NumberOfCats = numberOfCats,
                    NumberOfDogs = numberOfDogs,
                    RecognitionAttempts = totalScans,
                    MatchRate = matchRate,
                    TopUnmatchedScans = unmatchedScans,
                    ErrorBreakdown = errorTotal,

                    // FAR / FRR tiles show matched/unmatched scan counts (not true False
                    // Accept/Reject rates — those require a verified ground-truth label per
                    // scan, which this system doesn't capture).
                    DailyMissingPetScanFAR = matchedScans,
                    DailyMissingPetScanFRR = unmatchedScans,

                    TotalScans = totalScans,
                    MatchedScans = matchedScans,
                    UnmatchedScans = unmatchedScans,
                    ErrorBreakdownItems = errorBreakdownItems,

                    PetScanLogs = petScanLogViewModels,
                    TotalScanLogs = totalScanLogs,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    LstMonthlyUsers = lstMonthlyNewUsers,
                };

                response = SetResultStatus<DashboardViewModel>(dashboardDetails, Messages_Resources.Success, true);
            }
            catch (Exception ex)
            {
                var defaultDashboardModel = new DashboardViewModel
                {
                    TotalUsers = 0,
                    MonthlyNewUsers = 0,
                    NumberOfCats = 0,
                    NumberOfDogs = 0,
                    RecognitionAttempts = 0,
                    MatchRate = 0m,
                    TopUnmatchedScans = 0,
                    ErrorBreakdown = 0,

                    // FAR / FRR tiles — no data available on error
                    DailyMissingPetScanFAR = 0,
                    DailyMissingPetScanFRR = 0,

                    TotalScans = 0,
                    MatchedScans = 0,
                    UnmatchedScans = 0,
                    ErrorBreakdownItems = new List<ErrorBreakdownItem>(),
                    PetScanLogs = new List<PetScanLogViewModel>(),
                    TotalScanLogs = 0,
                    CurrentPage = 1,
                    PageSize = 10,
                    UserProfile = string.Empty,
                    UserName = string.Empty,
                    LstMonthlyUsers = new List<MonthlyUsers>(),
                };
                response = SetResultStatus<DashboardViewModel>(defaultDashboardModel, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Helper to build an ErrorBreakdownItem for a specific ErrorStage.
        /// </summary>
        private static ErrorBreakdownItem BuildErrorItem(
            string label,
            List<EnumRecognitionErrorStage> allErrors,
            EnumRecognitionErrorStage stage,
            int total)
        {
            var count = allErrors.Count(e => e == stage);
            return new ErrorBreakdownItem
            {
                Label = label,
                Count = count,
                Percentage = total > 0 ? Math.Round((decimal)count / total * 100m, 1) : 0m,
            };
        }

        public Task<ServiceResponse<DashboardViewModel>> GetDashboardDetails(Guid userId)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<List<RoleUserViewModel>> GetRoleInformation(JQueryDataTableModel param)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<RoleUserViewModel> GetRoleInformationById(Guid id)
        {
            throw new NotImplementedException();
        }

        public SelectList GetRoles()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<DashboardViewModel>> GetUserDashboardDetails(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<UserProfileResDTO>> UpdateProfile(UserProfileViewModel userModel)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<UserDashboardViewModel>> UserDashboard(Guid userID)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<ScanLogsPageViewModel>> GetScanLogsAsync(int page = 1, string search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var response = new ServiceResponse<ScanLogsPageViewModel>();
            try
            {
                int pageSize = 10;
                // Scan Log page shows only matched + no_match scans (scans that have a definitive result)
                var query = _unitOfWork.Instance.PetScans
                    .Where(s => s.MatchResult != null &&
                                (s.MatchResult.ToLower() == "matched" ||
                                 s.MatchResult.ToLower() == "match" ||
                                 s.MatchResult.ToLower() == "possible_match" ||
                                 s.MatchResult.ToLower() == "no_match" ||
                                 s.MatchResult.ToLower() == "unmatched" ||
                                 s.MatchResult.ToLower() == "unmatch"))
                    .AsQueryable();

                if (fromDate.HasValue)
                {
                    query = query.Where(s => s.CreatedOn >= fromDate.Value.Date);
                }
                if (toDate.HasValue)
                {
                    var toDateEnd = toDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(s => s.CreatedOn <= toDateEnd);
                }

                var petScanLogsQuery = query
                    .OrderByDescending(s => s.CreatedOn)
                    .Select(s => new
                    {
                        s.Id,
                        s.Species,
                        s.MatchResult,
                        s.MatchConfidence,
                        s.ClassifierConfidence,
                        s.CreatedOn,
                        s.ScanType,
                        s.Notes,
                        s.PetId,
                        s.MatchedDsId,
                    });

                var petScanLogs = await petScanLogsQuery.ToListAsync();

                var petInfoLookup = await GetPetInfoLookupAsync(
                    petScanLogs.Select(s => (s.PetId, s.MatchedDsId)));

                var petScanLogViewModels = petScanLogs.Select(s =>
                {
                    var effectivePetId = GetEffectivePetId(s.PetId, s.MatchedDsId);
                    var pet = effectivePetId.HasValue && petInfoLookup.TryGetValue(effectivePetId.Value, out var p) ? p : null;

                    return new PetScanLogViewModel
                    {
                        Id = s.Id,
                        PetName = pet?.PName ?? "Unknown",
                        PetType = s.Species.ToString(),
                        PetImagePath = !string.IsNullOrWhiteSpace(pet?.FullBodyImagePath)
                            ? ResolveImageUrl(pet.FullBodyImagePath)
                            : "/images/pet-placeholder.svg",
                        Result = s.MatchResult,
                        Confidence = (s.MatchConfidence ?? s.ClassifierConfidence ?? 0m) * 100m,
                        ScanDate = s.CreatedOn,
                        ScanType = s.ScanType.ToString(),
                        Notes = s.Notes,
                    };
                }).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var lowerSearch = search.Trim().ToLower();
                    petScanLogViewModels = petScanLogViewModels.Where(s =>
                        (s.PetName != null && s.PetName.ToLower().Contains(lowerSearch)) ||
                        (s.PetType != null && s.PetType.ToLower().Contains(lowerSearch)) ||
                        (s.ScanType != null && s.ScanType.ToLower().Contains(lowerSearch)) ||
                        (s.Result != null && s.Result.ToLower().Contains(lowerSearch))
                    ).ToList();
                }

                var model = new ScanLogsPageViewModel
                {
                    PetScanLogs = petScanLogViewModels,
                    TotalScanLogs = petScanLogViewModels.Count,
                    CurrentPage = page,
                    PageSize = pageSize,
                    SearchText = search,
                    FromDate = fromDate,
                    ToDate = toDate
                };

                response = SetResultStatus<ScanLogsPageViewModel>(model, Messages_Resources.Success, true);
            }
            catch (Exception ex)
            {
                response = SetResultStatus<ScanLogsPageViewModel>(new ScanLogsPageViewModel(), Messages_Resources.Error, false);
            }
            return response;
        }

        public async Task<ServiceResponse<List<PetScanLogViewModel>>> GetAllScanLogsAsync(string search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var response = new ServiceResponse<List<PetScanLogViewModel>>();
            try
            {
                // CSV export mirrors the Scan Log page: only matched + no_match rows
                var query = _unitOfWork.Instance.PetScans
                    .Where(s => s.MatchResult != null &&
                                (s.MatchResult.ToLower() == "matched" ||
                                 s.MatchResult.ToLower() == "match" ||
                                 s.MatchResult.ToLower() == "possible_match" ||
                                 s.MatchResult.ToLower() == "no_match" ||
                                 s.MatchResult.ToLower() == "unmatched" ||
                                 s.MatchResult.ToLower() == "unmatch"))
                    .AsQueryable();

                if (fromDate.HasValue)
                {
                    query = query.Where(s => s.CreatedOn >= fromDate.Value.Date);
                }
                if (toDate.HasValue)
                {
                    var toDateEnd = toDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(s => s.CreatedOn <= toDateEnd);
                }

                var petScanLogsQuery = query
                    .OrderByDescending(s => s.CreatedOn)
                    .Select(s => new
                    {
                        s.Id,
                        s.Species,
                        s.MatchResult,
                        s.MatchConfidence,
                        s.ClassifierConfidence,
                        s.CreatedOn,
                        s.ScanType,
                        s.Notes,
                        s.PetId,
                        s.MatchedDsId,
                    });

                var petScanLogs = await petScanLogsQuery.ToListAsync();

                var petInfoLookup = await GetPetInfoLookupAsync(
                    petScanLogs.Select(s => (s.PetId, s.MatchedDsId)));

                var petScanLogViewModels = petScanLogs.Select(s =>
                {
                    var effectivePetId = GetEffectivePetId(s.PetId, s.MatchedDsId);
                    var pet = effectivePetId.HasValue && petInfoLookup.TryGetValue(effectivePetId.Value, out var p) ? p : null;

                    return new PetScanLogViewModel
                    {
                        Id = s.Id,
                        PetName = pet?.PName ?? "Unknown",
                        PetType = s.Species.ToString(),
                        PetImagePath = !string.IsNullOrWhiteSpace(pet?.FullBodyImagePath)
                            ? ResolveImageUrl(pet.FullBodyImagePath)
                            : "/images/pet-placeholder.svg",
                        Result = s.MatchResult,
                        Confidence = (s.MatchConfidence ?? s.ClassifierConfidence ?? 0m) * 100m,
                        ScanDate = s.CreatedOn,
                        ScanType = s.ScanType.ToString(),
                        Notes = s.Notes,
                    };
                }).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var lowerSearch = search.Trim().ToLower();
                    petScanLogViewModels = petScanLogViewModels.Where(s =>
                        (s.PetName != null && s.PetName.ToLower().Contains(lowerSearch)) ||
                        (s.PetType != null && s.PetType.ToLower().Contains(lowerSearch)) ||
                        (s.ScanType != null && s.ScanType.ToLower().Contains(lowerSearch)) ||
                        (s.Result != null && s.Result.ToLower().Contains(lowerSearch))
                    ).ToList();
                }

                response = SetResultStatus<List<PetScanLogViewModel>>(petScanLogViewModels, Messages_Resources.Success, true);
            }
            catch (Exception ex)
            {
                response = SetResultStatus<List<PetScanLogViewModel>>(new List<PetScanLogViewModel>(), Messages_Resources.Error, false);
            }
            return response;
        }
    }
}


