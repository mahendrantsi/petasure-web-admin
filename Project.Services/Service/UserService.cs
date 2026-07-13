using Microsoft.AspNetCore.Mvc.Rendering;
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
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

                // Generate dummy pet scan logs
                var petScanLogs = new List<PetScanLogViewModel>
                {
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Buddy",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Matched",
                        Confidence = 97.5m,
                        ScanDate = DateTime.Now.AddMinutes(-22)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Luna",
                        PetType = "Cat",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Matched",
                        Confidence = 89.0m,
                        ScanDate = DateTime.Now.AddMinutes(-44)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Charlie",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Unmatched",
                        Confidence = 0m,
                        ScanDate = DateTime.Now.AddMinutes(-66)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Whiskers",
                        PetType = "Cat",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Matched",
                        Confidence = 91.0m,
                        ScanDate = DateTime.Now.AddMinutes(-85)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Max",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Unmatched",
                        Confidence = 0m,
                        ScanDate = DateTime.Now.AddMinutes(-100)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Milo",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Matched",
                        Confidence = 87.0m,
                        ScanDate = DateTime.Now.AddMinutes(-121)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Lily",
                        PetType = "Cat",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Matched",
                        Confidence = 90.0m,
                        ScanDate = DateTime.Now.AddMinutes(-145)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Rocky",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Unmatched",
                        Confidence = 0m,
                        ScanDate = DateTime.Now.AddMinutes(-175)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Puppy",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Matched",
                        Confidence = 0m,
                        ScanDate = DateTime.Now.AddMinutes(-175)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Shadow",
                        PetType = "Cat",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Matched",
                        Confidence = 92.0m,
                        ScanDate = DateTime.Now.AddMinutes(-200)
                    },
                    new PetScanLogViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Daisy",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        Result = "Matched",
                        Confidence = 88.0m,
                        ScanDate = DateTime.Now.AddMinutes(-220)
                    },
                };

                // Generate dummy ill-health reviews
                var illHealthReviews = new List<IllHealthReviewViewModel>
                {
                    new IllHealthReviewViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Buddy",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        AISuggestedCondition = "Skin Infection (Dermatitis)",
                        Confidence = 85m,
                        Status = "Flagged Submission",
                        AIVerdict = "Ill",
                        AdminOverride = "Select",
                        OverrideNotes = "Add notes here...",
                        SubmissionDate = DateTime.Now.AddHours(-2)
                    },
                    new IllHealthReviewViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Luna",
                        PetType = "Cat",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        AISuggestedCondition = "Ear Infection (Otitis)",
                        Confidence = 78m,
                        Status = "Reviewed",
                        AIVerdict = "Ill",
                        AdminOverride = "Healthy",
                        OverrideNotes = "Recovered after medication",
                        SubmissionDate = DateTime.Now.AddHours(-4)
                    },
                    new IllHealthReviewViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Max",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        AISuggestedCondition = "ARM Fever (Pyreal)",
                        Confidence = 65m,
                        Status = "Flagged Submission",
                        AIVerdict = "Ill",
                        AdminOverride = "Select",
                        OverrideNotes = "Add notes here...",
                        SubmissionDate = DateTime.Now.AddHours(-6)
                    },
                    new IllHealthReviewViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Milo",
                        PetType = "Cat",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        AISuggestedCondition = "Vomiting (Gastroenteritis)",
                        Confidence = 81m,
                        Status = "Reviewed",
                        AIVerdict = "Ill",
                        AdminOverride = "Ill",
                        OverrideNotes = "Continue treatment",
                        SubmissionDate = DateTime.Now.AddHours(-8)
                    },
                    new IllHealthReviewViewModel
                    {
                        Id = Guid.NewGuid(),
                        PetName = "Rocky",
                        PetType = "Dog",
                        PetImagePath = "~/images/pet-placeholder.svg",
                        AISuggestedCondition = "Eye Irritation (Conjunctivitis)",
                        Confidence = 72m,
                        Status = "Resolved",
                        AIVerdict = "Ill",
                        AdminOverride = "Healthy",
                        OverrideNotes = "Resolved",
                        SubmissionDate = DateTime.Now.AddHours(-10)
                    }
                };

                var dashboardDetails = new DashboardViewModel()
                {
                    TotalUsers = totalUser,
                    MonthlyNewUsers = monthlyNewUsers,
                    NumberOfCats = numberOfCats,
                    NumberOfDogs = numberOfDogs,
                    RecognitionAttempts = 2450,
                    MatchRate = 87.5m,
                    TopUnmatchedScans = 312,
                    ErrorBreakdown = 18,
                    PetScanLogs = petScanLogs,
                    FlaggedSubmissions = 14,
                    UnderReview = 5,
                    Reviewed = 6,
                    Resolved = 3,
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
    }
}
