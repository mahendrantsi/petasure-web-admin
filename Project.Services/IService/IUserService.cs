
namespace Project.Services.IService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Project.Models.AdminModel;
    using Project.Models.CommonModel;
    using Project.Services.ServiceEntities;
    using Project.Models.AccountModel;
    using Project.Data.DBEntities;
    using Project.Models.ProfileModel;
    using Project.Models.Dashboard;
    using System;

    public interface IUserService
    {

        /// <summary>
        /// Get Role Information.
        /// </summary>
        /// <returns>List of Roles.</returns>
        ServiceResponse<List<RoleUserViewModel>> GetRoleInformation(JQueryDataTableModel param);

        /// <summary>
        /// Create New Role.
        /// </summary>
        /// <param name="model">Role View Model.</param>
        /// <returns>Created Role Model.</returns>
        //Task<ServiceResponse<RoleUserViewModel>> SaveRole(RoleUserViewModel model);

        /// <summary>
        /// Get Role Information by Id.
        /// </summary>
        /// <param name="id">Role Id.</param>
        /// <returns>Role detail view.</returns>
        ServiceResponse<RoleUserViewModel> GetRoleInformationById(Guid id);


        SelectList GetRoles();

        Task<ServiceResponse<UserProfileResDTO>> UpdateProfile(UserProfileViewModel userModel);


        /// <summary>
        /// Get Dashboard Details
        /// </summary>
        /// <param name="userId">int</param>
        /// <returns>List<DashboardViewModel></returns>
        Task<ServiceResponse<DashboardViewModel>> GetDashboardDetails(Guid userId);

        Task<ServiceResponse<DashboardViewModel>> GetUserDashboardDetails(Guid userId);
        Task<ServiceResponse<DashboardViewModel>> GetAdminDashboard();
        Task<ServiceResponse<UserProfile>> DeleteProfile(Guid userId);


        /// <summary>
        /// GET USER DASHBOARD DETAILS
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>UserDashboardViewModel</returns>
        Task<ServiceResponse<UserDashboardViewModel>> UserDashboard(Guid userID);
    }
}
