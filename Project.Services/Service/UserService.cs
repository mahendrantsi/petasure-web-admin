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



                var lstMonthlyNewUsers = _unitOfWork.GenericRepository<DerivedIdentityUser>().Get()
                                        .GroupBy(u => new { u.CreatedOn.Year, u.CreatedOn.Month })
                                        .Select(g => new MonthlyUsers
                                        {
                                            Year = g.Key.Year,
                                            Month = g.Key.Month,
                                            UserCount = g.Count()
                                        })
                                        .OrderBy(result => result.Year).ThenBy(result => result.Month)
                                        .ToList();

                var dashboardDetails = new DashboardViewModel()
                {
                    TotalUsers = totalUser,
                    MonthlyNewUsers = monthlyNewUsers,
                    LstMonthlyUsers = lstMonthlyNewUsers
                };

                response = SetResultStatus<DashboardViewModel>(dashboardDetails, Messages_Resources.Success, true);
            }
            catch (Exception ex)
            {
                response = SetResultStatus<DashboardViewModel>(null, Messages_Resources.Error, false);
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
