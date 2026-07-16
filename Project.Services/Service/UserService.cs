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
            return _imageBaseUrl + "/" + relativePath.TrimStart('/');
        }
        public Task<ServiceResponse<UserProfile>> DeleteProfile(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<DashboardViewModel>> GetAdminDashboard()
        {
            var response = new ServiceResponse<DashboardViewModel>();
            try
            {
                var totalUser =  _unitOfWork.UserAccountRepository.GetTotalUsers();

                var monthlyNewUsers =  _unitOfWork.UserAccountRepository.GetMonthlyNewUsers();



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

                // Most recent recognition scans (register/similar/analyze/classify), newest first.
                var petScanLogs = _unitOfWork.Instance.PetScans
                    .Include(s => s.Pet)
                    .Include(s => s.PrimaryImage)
                    .OrderByDescending(s => s.CreatedOn)
                    .Take(20)
                    .AsEnumerable()
                    .Select(s => new PetScanLogViewModel
                    {
                        Id = s.Id,
                        PetName = s.Pet != null ? s.Pet.PName : "Unknown",
                        PetType = s.Species.ToString(),
                        PetImagePath = s.PrimaryImage != null ? ResolveImageUrl(s.PrimaryImage.StoragePath) : "~/images/pet-placeholder.svg",
                        Result = s.MatchResult ?? s.RouteDecision ?? s.Status.ToString(),
                        Confidence = (s.MatchConfidence ?? s.ClassifierConfidence ?? 0m) * 100m,
                        ScanDate = s.CreatedOn,
                    })
                    .ToList();

                // Most recent illness-check events, newest first.
                var illHealthReviews = _unitOfWork.Instance.HealthCheckEvents
                    .Include(e => e.Pet)
                    .Include(e => e.HealthStatuses)
                    .OrderByDescending(e => e.CreatedOn)
                    .Take(20)
                    .AsEnumerable()
                    .Select(e =>
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
                            // HealthCheckEvent/HealthStatus don't yet track an admin override
                            // decision separately from Status — "Select" mirrors the UI's own
                            // not-yet-reviewed placeholder until that workflow is added.
                            AdminOverride = "Select",
                            OverrideNotes = string.Empty,
                            SubmissionDate = e.SubmittedAt,
                        };
                    })
                    .ToList();

                var recognitionAttempts = _unitOfWork.Instance.PetScans.Count();

                var similarScans = _unitOfWork.Instance.PetScans
                    .Where(s => s.ScanType == EnumPetScanType.Similar)
                    .ToList();
                var matchRate = similarScans.Any()
                    ? (decimal)similarScans.Count(s => s.MatchResult == "matched") / similarScans.Count * 100m
                    : 0m;
                var topUnmatchedScans = similarScans.Count(s => s.MatchResult == "no_match");

                // Rolling 30-day window — the original mock didn't specify a period.
                var errorWindowStart = DateTime.UtcNow.AddDays(-30);
                var errorBreakdown = _unitOfWork.Instance.RecognitionErrors.Count(e => e.CreatedOn >= errorWindowStart);

                // HealthCheckEvent only tracks 3 lifecycle states (Pending/Reviewed/Closed);
                // the dashboard wants 4 buckets, so Pending is split by whether the AI found
                // anything — a temporary heuristic until a real admin-review workflow exists.
                var flaggedSubmissions = _unitOfWork.Instance.HealthCheckEvents
                    .Count(e => e.Status == EnumHealthCheckStatus.Pending && e.HealthStatuses.Any());
                var underReview = _unitOfWork.Instance.HealthCheckEvents
                    .Count(e => e.Status == EnumHealthCheckStatus.Pending && !e.HealthStatuses.Any());
                var reviewed = _unitOfWork.Instance.HealthCheckEvents
                    .Count(e => e.Status == EnumHealthCheckStatus.Reviewed);
                var resolved = _unitOfWork.Instance.HealthCheckEvents
                    .Count(e => e.Status == EnumHealthCheckStatus.Closed);

                var dashboardDetails = new DashboardViewModel()
                {
                    TotalUsers = totalUser,
                    MonthlyNewUsers = monthlyNewUsers,
                    NumberOfCats = numberOfCats,
                    NumberOfDogs = numberOfDogs,
                    RecognitionAttempts = recognitionAttempts,
                    MatchRate = matchRate,
                    TopUnmatchedScans = topUnmatchedScans,
                    ErrorBreakdown = errorBreakdown,
                    PetScanLogs = petScanLogs,
                    FlaggedSubmissions = flaggedSubmissions,
                    UnderReview = underReview,
                    Reviewed = reviewed,
                    Resolved = resolved,
                    IllHealthReviews = illHealthReviews,
                    LstMonthlyUsers = lstMonthlyNewUsers
                };

                response = SetResultStatus<DashboardViewModel>(dashboardDetails, Messages_Resources.Success, true);
            }
            catch (Exception ex)
            {
                // Create a default empty dashboard model instead of null to prevent NullReferenceException in the view
                var defaultDashboardModel = new DashboardViewModel()
                {
                    TotalUsers = 0,
                    MonthlyNewUsers = 0,
                    NumberOfCats = 0,
                    NumberOfDogs = 0,
                    RecognitionAttempts = 0,
                    MatchRate = 0m,
                    TopUnmatchedScans = 0,
                    ErrorBreakdown = 0,
                    PetScanLogs = new List<PetScanLogViewModel>(),
                    FlaggedSubmissions = 0,
                    UnderReview = 0,
                    Reviewed = 0,
                    Resolved = 0,
                    IllHealthReviews = new List<IllHealthReviewViewModel>(),
                    UserProfile = string.Empty,
                    UserName = string.Empty,
                    LstMonthlyUsers = new List<MonthlyUsers>()
                };
                response = SetResultStatus<DashboardViewModel>(defaultDashboardModel, Messages_Resources.Error, false);
            }
            return response;
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
