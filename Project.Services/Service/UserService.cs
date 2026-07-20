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

                // ── Scan metrics across all scan types ──────────────────────────────────
                var allScans = await _unitOfWork.Instance.PetScans
                    .Select(s => new { s.MatchResult, s.Status })
                    .ToListAsync();

                var totalScans = allScans.Count;
                var matchedScans = allScans.Count(s => s.MatchResult == "matched" || s.MatchResult == "possible_match");
                var unmatchedScans = allScans.Count(s => s.MatchResult == "no_match");
                var errorScans = allScans.Count(s => s.Status == EnumPetScanStatus.Failed || s.Status == EnumPetScanStatus.Rejected);

                // ── Error breakdown by stage ────────────────────────────────────────────
                var allErrors = await _unitOfWork.Instance.RecognitionErrors
                    .Select(e => e.ErrorStage)
                    .ToListAsync();

                var errorTotal = allErrors.Count;
                var errorBreakdownItems = new List<ErrorBreakdownItem>
                {
                    BuildErrorItem("Image Quality",    allErrors, EnumRecognitionErrorStage.ImageSave,       errorTotal),
                    BuildErrorItem("No Face Detected", allErrors, EnumRecognitionErrorStage.RecognitionGate, errorTotal),
                    BuildErrorItem("AI Request Error", allErrors, EnumRecognitionErrorStage.AiRequest,       errorTotal),
                    BuildErrorItem("Response Parse",   allErrors, EnumRecognitionErrorStage.AiResponseParse, errorTotal),
                    BuildErrorItem("System Error",     allErrors, EnumRecognitionErrorStage.DbSave,          errorTotal),
                };

                // ── Pet Scan Logs — server-side paginated ───────────────────────────────
                var petScanLogsQuery = from s in _unitOfWork.Instance.PetScans
                                       join p in _unitOfWork.Instance.PetInfo on s.PetId equals p.Id into petGroup
                                       from pet in petGroup.DefaultIfEmpty()
                                       orderby s.CreatedOn descending
                                       select new
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
                                           PetName = pet != null ? pet.PName : "Unknown",
                                           PetFullBodyImagePath = pet != null ? pet.FullBodyImagePath : null,
                                       };

                var totalScanLogs = await petScanLogsQuery.CountAsync();
                var petScanLogs = await petScanLogsQuery
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var petScanLogViewModels = petScanLogs.Select(s => new PetScanLogViewModel
                {
                    Id = s.Id,
                    PetName = s.PetName,
                    PetType = s.Species.ToString(),
                    // Use the pet's FullBodyImagePath from petinfo table (not the scan image)
                    PetImagePath = !string.IsNullOrWhiteSpace(s.PetFullBodyImagePath)
                        ? ResolveImageUrl(s.PetFullBodyImagePath)
                        : "/images/pet-placeholder.svg",
                    Result = s.MatchResult ?? s.RouteDecision ?? s.Status.ToString(),
                    Confidence = (s.MatchConfidence ?? s.ClassifierConfidence ?? 0m) * 100m,
                    ScanDate = s.CreatedOn,
                    ScanType = s.ScanType.ToString(),
                    Notes = s.Notes,
                }).ToList();

                // ── Ill-health reviews ──────────────────────────────────────────────────
                var illHealthReviews = await _unitOfWork.Instance.HealthCheckEvents
                    .Include(e => e.Pet)
                    .Include(e => e.HealthStatuses)
                    .OrderByDescending(e => e.CreatedOn)
                    .Take(20)
                    .ToListAsync();

                var illHealthReviewViewModels = illHealthReviews.Select(e =>
                {
                    var topFinding = e.HealthStatuses.OrderByDescending(h => h.Confidence).FirstOrDefault();
                    var hasFindings = e.HealthStatuses.Any();
                    return new IllHealthReviewViewModel
                    {
                        Id = e.Id,
                        PetName = e.Pet != null ? e.Pet.PName : "Unknown",
                        PetType = e.Species.ToString(),
                        PetImagePath = ResolveImageUrl(e.ImageRef),
                        AISuggestedCondition = topFinding?.ConditionName ?? (e.AiSummary ?? "No concerns detected"),
                        Confidence = (topFinding?.Confidence ?? 0m) * 100m,
                        Status = MapHealthCheckReviewStatus(e.Status, hasFindings),
                        AIVerdict = hasFindings ? "Ill" : "Healthy",
                        AdminOverride = "Select",
                        OverrideNotes = string.Empty,
                        SubmissionDate = e.SubmittedAt,
                    };
                }).ToList();

                // ── Match rate across all scan types ───────────────────────────────────
                var matchRate = totalScans > 0
                    ? (decimal)matchedScans / totalScans * 100m
                    : 0m;

                // ── Health-check buckets ────────────────────────────────────────────────
                var flaggedSubmissions = await _unitOfWork.Instance.HealthCheckEvents
                    .CountAsync(e => e.Status == EnumHealthCheckStatus.Pending && e.HealthStatuses.Any());
                var underReview = await _unitOfWork.Instance.HealthCheckEvents
                    .CountAsync(e => e.Status == EnumHealthCheckStatus.Pending && !e.HealthStatuses.Any());
                var reviewed = await _unitOfWork.Instance.HealthCheckEvents
                    .CountAsync(e => e.Status == EnumHealthCheckStatus.Reviewed);
                var resolved = await _unitOfWork.Instance.HealthCheckEvents
                    .CountAsync(e => e.Status == EnumHealthCheckStatus.Closed);

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

                    TotalScans = totalScans,
                    MatchedScans = matchedScans,
                    UnmatchedScans = unmatchedScans,
                    ErrorCount = errorScans,
                    ErrorBreakdownItems = errorBreakdownItems,

                    PetScanLogs = petScanLogViewModels,
                    TotalScanLogs = totalScanLogs,
                    CurrentPage = currentPage,
                    PageSize = pageSize,

                    FlaggedSubmissions = flaggedSubmissions,
                    UnderReview = underReview,
                    Reviewed = reviewed,
                    Resolved = resolved,
                    IllHealthReviews = illHealthReviewViewModels,
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
                    TotalScans = 0,
                    MatchedScans = 0,
                    UnmatchedScans = 0,
                    ErrorCount = 0,
                    ErrorBreakdownItems = new List<ErrorBreakdownItem>(),
                    PetScanLogs = new List<PetScanLogViewModel>(),
                    TotalScanLogs = 0,
                    CurrentPage = 1,
                    PageSize = 10,
                    FlaggedSubmissions = 0,
                    UnderReview = 0,
                    Reviewed = 0,
                    Resolved = 0,
                    IllHealthReviews = new List<IllHealthReviewViewModel>(),
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
                var query = _unitOfWork.Instance.PetScans.AsQueryable();

                if (fromDate.HasValue)
                {
                    query = query.Where(s => s.CreatedOn >= fromDate.Value.Date);
                }
                if (toDate.HasValue)
                {
                    var toDateEnd = toDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(s => s.CreatedOn <= toDateEnd);
                }

                var petScanLogsQuery = from s in query
                                       join p in _unitOfWork.Instance.PetInfo on s.PetId equals p.Id into petGroup
                                       from pet in petGroup.DefaultIfEmpty()
                                       orderby s.CreatedOn descending
                                       select new
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
                                           PetName = pet != null ? pet.PName : "Unknown",
                                           PetFullBodyImagePath = pet != null ? pet.FullBodyImagePath : null,
                                       };

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var lowerSearch = search.Trim().ToLower();
                    petScanLogsQuery = petScanLogsQuery.Where(s => 
                        (s.PetName != null && s.PetName.ToLower().Contains(lowerSearch)) ||
                        (s.MatchResult != null && s.MatchResult.ToLower().Contains(lowerSearch)) ||
                        (s.RouteDecision != null && s.RouteDecision.ToLower().Contains(lowerSearch))
                    );
                }

                var totalScanLogs = await petScanLogsQuery.CountAsync();
                var petScanLogs = await petScanLogsQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var petScanLogViewModels = petScanLogs.Select(s => new PetScanLogViewModel
                {
                    Id = s.Id,
                    PetName = s.PetName,
                    PetType = s.Species.ToString(),
                    PetImagePath = !string.IsNullOrWhiteSpace(s.PetFullBodyImagePath)
                        ? ResolveImageUrl(s.PetFullBodyImagePath)
                        : "/images/pet-placeholder.svg",
                    Result = s.MatchResult ?? s.RouteDecision ?? s.Status.ToString(),
                    Confidence = (s.MatchConfidence ?? s.ClassifierConfidence ?? 0m) * 100m,
                    ScanDate = s.CreatedOn,
                    ScanType = s.ScanType.ToString(),
                    Notes = s.Notes,
                }).ToList();

                var model = new ScanLogsPageViewModel
                {
                    PetScanLogs = petScanLogViewModels,
                    TotalScanLogs = totalScanLogs,
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
                var query = _unitOfWork.Instance.PetScans.AsQueryable();

                if (fromDate.HasValue)
                {
                    query = query.Where(s => s.CreatedOn >= fromDate.Value.Date);
                }
                if (toDate.HasValue)
                {
                    var toDateEnd = toDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(s => s.CreatedOn <= toDateEnd);
                }

                var petScanLogsQuery = from s in query
                                       join p in _unitOfWork.Instance.PetInfo on s.PetId equals p.Id into petGroup
                                       from pet in petGroup.DefaultIfEmpty()
                                       orderby s.CreatedOn descending
                                       select new
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
                                           PetName = pet != null ? pet.PName : "Unknown",
                                           PetFullBodyImagePath = pet != null ? pet.FullBodyImagePath : null,
                                       };

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var lowerSearch = search.Trim().ToLower();
                    petScanLogsQuery = petScanLogsQuery.Where(s => 
                        (s.PetName != null && s.PetName.ToLower().Contains(lowerSearch)) ||
                        (s.MatchResult != null && s.MatchResult.ToLower().Contains(lowerSearch)) ||
                        (s.RouteDecision != null && s.RouteDecision.ToLower().Contains(lowerSearch))
                    );
                }

                var petScanLogs = await petScanLogsQuery.ToListAsync();

                var petScanLogViewModels = petScanLogs.Select(s => new PetScanLogViewModel
                {
                    Id = s.Id,
                    PetName = s.PetName,
                    PetType = s.Species.ToString(),
                    PetImagePath = !string.IsNullOrWhiteSpace(s.PetFullBodyImagePath)
                        ? ResolveImageUrl(s.PetFullBodyImagePath)
                        : "/images/pet-placeholder.svg",
                    Result = s.MatchResult ?? s.RouteDecision ?? s.Status.ToString(),
                    Confidence = (s.MatchConfidence ?? s.ClassifierConfidence ?? 0m) * 100m,
                    ScanDate = s.CreatedOn,
                    ScanType = s.ScanType.ToString(),
                    Notes = s.Notes,
                }).ToList();

                response = SetResultStatus<List<PetScanLogViewModel>>(petScanLogViewModels, Messages_Resources.Success, true);
            }
            catch (Exception ex)
            {
                response = SetResultStatus<List<PetScanLogViewModel>>(new List<PetScanLogViewModel>(), Messages_Resources.Error, false);
            }
            return response;
        }

        private static string MapHealthCheckReviewStatus(EnumHealthCheckStatus status, bool hasFindings)
        {
            return status switch
            {
                EnumHealthCheckStatus.Pending => hasFindings ? "Flagged Submission" : "Under Review",
                EnumHealthCheckStatus.Reviewed => "Reviewed",
                EnumHealthCheckStatus.Closed => "Resolved",
                _ => "Under Review",
            };
        }
    }
}
