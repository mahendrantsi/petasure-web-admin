using Microsoft.AspNetCore.Http;
using Project.Core.Enum;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.AccountModel;
using Project.Models.AdminModel;
using Project.Models.CommonModel;
using Project.Models.Pets;
using Project.Models.User;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IAccountService
    {
        Task<ServiceResponse<RegisterCustomerResDTO>> RegisterCustomer(RegisterViewModel dto);

        Task<ServiceResponse<string>> AddSecondaryUser(SecondaryUserDTO secondaryUser, Guid parentId);

        Task<ServiceResponse<GUIDViewModel>> CreateIDCheckUser(FoundMissingPetRequest foundMissingPetRequest);
        Task<ServiceResponse<LoginResult>> Login(LoginReqDTO loginViewModel, bool checkAdmin = true);
        Task<ServiceResponse<EnableAuthenticatorViewModel>> LoadSharedKeyAndQrCodeUriAsync(DerivedIdentityUser user, EnableAuthenticatorViewModel model);
        Task<ServiceResponse<string>> GetUserRole(string userName);
        Task<ServiceResponse<ResetPasswordViewModel>> ResetPassword(ResetPasswordViewModel model);
        Task<ServiceResponse<string>> ChangePassword(Guid userId, string currentPassword, string newPassword);
        Task<ServiceResponse<string>> UpdateDeviceDetails(Guid UserId, string Token, string deviceType);
        Task<ServiceResponse<DerivedIdentityUser>> GetUserById(string userId);
        Task<ServiceResponse<RegisterViewModel>> CreateUserWithProfile(RegisterViewUserModel dto, Guid userId);
        Task<ServiceResponse<RegisterViewModel>> UpdateUser(RegisterViewUserModel dto);
        Task<ServiceResponse<DerivedIdentityUser>> UpdateProfile(EditUserRequestViewModel dto, Guid userID);
        Task<ServiceResponse<RegisterViewUserModel>> GetProfileDetails(string id);
        Task<DerivedIdentityUser> FindUserByEmailID(string email);
        Task<ServiceResponse<UserProfile>> GetUserProfileByUserID(Guid userId);
        Task<ServiceResponse<List<UserDetailModel>>> GetUsers(UserListFilterModel requestParam);
        Task<ServiceResponse<DerivedIdentityUser>> DeleteUser(string id);
        Task<ServiceResponse<DerivedIdentityUser>> DeleteUserPermanent(string id);
        Task LogOutAsync();
        Task LogOutAsync(DerivedIdentityUser user, HttpContext context);
        Task<ServiceResponse<UserProfileViewModel>> GetUserByReferralCode(string referral);
        Task<ServiceResponse<DerivedIdentityUser>> GetByEmailAsync(string email);
        Task<ServiceResponse<ForgotPasswordResponseModel>> ForgotPasswordLink(string username, string baseUrl);
        Task<ServiceResponse<ForgotPasswordResponseModel>> CreatePasswordLink(string username, string baseUrl);
        Task<ServiceResponse<IList<UserViewModel>>> GetUserByRole(string Role);
        Task<ServiceResponse<RegisterViewModel>> CreateUser(RegisterViewModel dto);

        Task<ServiceResponse<DerivedIdentityUser>> GetUserDetailById(Guid userId);

        Task<ServiceResponse<string>> SaveUploadedImagesAsync(Microsoft.AspNetCore.Http.IFormFile userImage, string foldername);
        EnumUserType GetUserType(Guid userid);
    }
}
