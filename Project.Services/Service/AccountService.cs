using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Extensions.Msal;
using Project.Core.ActionFilter;
using Project.Core.Enum;
using Project.Core.Extension;
using Project.Core.Settings;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.AccountModel;
using Project.Models.AdminModel;
using Project.Models.AdminRule;
using Project.Models.CommonModel;
using Project.Models.Pets;
using Project.Models.ProfileModel;
using Project.Models.User;
using Project.Persistence.UOW;
using Project.Services.IService;
using Project.Services.Resources;
using Project.Services.ServiceEntities;
using ServiceStack;
using ServiceStack.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer.Utilities;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Project.Services.Service
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly SignInManager<DerivedIdentityUser> _signInManager;
        private readonly UserManager<DerivedIdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private readonly UrlEncoder _urlEncoder;
        private readonly IHistoryService _historyService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _setting;
        private readonly ISystemSetting _System;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IPetService petService;
        private readonly ISubscriptionService subscriptionService;

        public AccountService(UserManager<DerivedIdentityUser> userManager, SignInManager<DerivedIdentityUser> signInManager, IUnitOfWork unitOfWork, UrlEncoder urlEncoder,
                             IHistoryService historyService, IWebHostEnvironment webHostEnvironment, IMapper mapper, ISettingService settings, ISystemSetting system,
                             IEmailService emailService, IConfiguration configuration, IPetService petService, ISubscriptionService subscriptionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _urlEncoder = urlEncoder;
            _historyService = historyService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _setting = settings;
            _System = system;
            _emailService = emailService;
            _configuration = configuration;
            this.petService = petService;
            this.subscriptionService = subscriptionService;
        }

        public async Task<ServiceResponse<RegisterCustomerResDTO>> RegisterCustomer(RegisterViewModel dto)
        {
            ServiceResponse<RegisterCustomerResDTO> objReturn;
            try
            {

                DerivedIdentityUser existingUser;

                existingUser = await this._userManager.FindByNameAsync(dto.Username);
                if (existingUser is not null)
                    return this.SetResultStatus<RegisterCustomerResDTO>(null, Messages_Resources.UsernameAlreadyExists, false);

                existingUser = await this._userManager.FindByEmailAsync(dto.Email);
                if (existingUser is not null)
                    return this.SetResultStatus<RegisterCustomerResDTO>(null, Messages_Resources.EmailAlreadyRegisteredWithUs, false);


                var userModel = new UserRegister
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    UserName = dto.Username,
                    Email = dto.Email,
                    Password = dto.Password,
                    Active = true,
                    PhoneNumber = dto.PhoneNumber,
                    CreatedOn = DateTime.UtcNow,
                    CustomerGuid = Guid.NewGuid(),
                    Role = (!string.IsNullOrEmpty(dto.Role) ? dto.Role : EnumRole.User.ToString()),
                    MobileCountryCode = dto.MobileCountryCode,
                    ShopifyId = dto.ShopifyId,
                    ShopifyResponse = dto.ShopifyResponse,
                    UserType = dto.UserType,
                    Address = dto.Address
                };

                var RegisteredUser = await _unitOfWork.UserAccountRepository.CreateAccount(userModel);
                await _historyService.SaveUserHistory(RegisteredUser);

                if (RegisteredUser.IsSuccess)
                {
                    objReturn = this.SetResultStatus<RegisterCustomerResDTO>(_mapper.Map<RegisterCustomerResDTO>(RegisteredUser), MessageStatus.Success, true);
                }
                else
                {
                    if (RegisteredUser.error.Count > 0)
                        objReturn = this.SetResultStatus<RegisterCustomerResDTO>(null, RegisteredUser.error[1], false);
                    else
                        objReturn = this.SetResultStatus<RegisterCustomerResDTO>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<RegisterCustomerResDTO>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }

        public async Task<ServiceResponse<LoginResult>> Login(LoginReqDTO loginViewModel, bool checkAdmin = true)
        {
            try
            {
                DerivedIdentityUser user;
                user = await this._userManager.FindByEmailAsync(loginViewModel.Username);
                if (user == null)
                    return this.SetResultStatus<LoginResult>(null, Messages_Resources.InvalidUsernamePassword, false);
                else if (!(await this._userManager.CheckPasswordAsync(user, loginViewModel.Password)))
                    return this.SetResultStatus<LoginResult>(null, Messages_Resources.InvalidUsernamePassword, false);
                if (user.IsDeleted == true || user.IsActive == false)
                    return this.SetResultStatus<LoginResult>(null, MessageStatus.InactiveAccount, false);

                var roles = await this._userManager.GetRolesAsync(user);
                if (checkAdmin &&  (roles.Contains(EnumRole.Admin.ToString()) || roles.Contains(EnumRole.SubAdmin.ToString())))
                    return this.SetResultStatus<LoginResult>(null, MessageStatus.AdminAccounts, false);

                var ObjResult = await this._signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
                if (ObjResult.Succeeded)
                {
                    this.UpdateDeviceDetails(user.Id, loginViewModel.FCMToken, loginViewModel.DeviceType);

                    return this.SetResultStatus<LoginResult>(new LoginResult(user), MessageStatus.Success, true);
                }
                else if (ObjResult.RequiresTwoFactor)
                {
                    return this.SetResultStatus<LoginResult>(new LoginResult(user), Convert.ToBoolean(user.IsDeviceConnected) ? MessageStatus.EnableAuthenticator : MessageStatus.RequiresTwoFactor, false);
                }
                else
                {
                    return this.SetResultStatus<LoginResult>(null, Messages_Resources.InvalidUsernamePassword, false);
                }

            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                throw;
            }
        }

        public async Task<ServiceResponse<LoginResult>> CheckUserRules(Guid userId)
        {
            try
            {
                DerivedIdentityUser user;
                user = await this._userManager.FindByIdAsync(userId.ToString());

                if (user is null)
                    return this.SetResultStatus<LoginResult>(null, Messages_Resources.InvalidUsernamePassword, false);

                AdminRule rule = new AdminRule(user, _System.GetSystemVariables(), (await this._userManager.GetRolesAsync(user))[0]);
                if (!rule.UserOnboarding)
                    return this.SetResultStatus<LoginResult>(null, rule.Errors[0], false);
                return this.SetResultStatus<LoginResult>(new LoginResult(user), MessageStatus.Success, true);

            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
            }
            return this.SetResultStatus<LoginResult>(null, MessageStatus.Error, false);

        }

        public async Task<ServiceResponse<string>> UpdateDeviceDetails(Guid UserId, string Token, string deviceType)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var userProfile = this._unitOfWork.Instance.UserProfile.FirstOrDefault(x => x.UserId == UserId);
                if (userProfile != null)
                {
                    userProfile.FCMToken = Token;
                    userProfile.DeviceType = deviceType;
                    this._unitOfWork.Instance.Update(userProfile);
                    var saveRes = this._unitOfWork.SaveChanges();

                    if (saveRes)
                        response = SetResultStatus<string>("Successful", Messages_Resources.Success, true);
                    else
                        response = SetResultStatus<string>("Failed", Messages_Resources.NotExists, false);
                }
                else
                {
                    response = SetResultStatus<string>(Messages_Resources.UserNotFound, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                response = SetResultStatus<string>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> ChangePassword(Guid userId, string currentPassword, string newPassword)
        {
            var user = await this.GetByIdAsync(userId);
            if (user == null)
            {
                return SetResultStatus(string.Empty, Messages_Resources.UserNotFound, false);
            }

            if (!await isPasswordMatch(user, currentPassword))
            {
                return SetResultStatus(string.Empty, Messages_Resources.InvalidCurrentPassword, false);
            }

            if (await isPasswordMatch(user, newPassword))
            {
                return SetResultStatus(string.Empty, Messages_Resources.OldNewPasswordMatch, false);
            }

            var response = await this._userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return response.Succeeded switch
            {
                true => SetResultStatus(string.Empty, Messages_Resources.PasswordUpdatedSuccess, true),
                _ => SetResultStatus(string.Empty, Messages_Resources.PasswordUpdatedError, false),
            };
        }

        public async Task<DerivedIdentityUser> GetByIdAsync(Guid userId) => await this._userManager.FindByIdAsync(userId.ToString());

        public async Task<ServiceResponse<DerivedIdentityUser>> GetByEmailAsync(string email)
        {
            var user = await this._userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return this.SetResultStatus<DerivedIdentityUser>(user, MessageStatus.Success, true);
            }
            else
            {
                return this.SetResultStatus<DerivedIdentityUser>(null, MessageStatus.Fail, false);
            }
        }

        public async Task<ServiceResponse<DerivedIdentityUser>> GetUserDetailById(Guid userId)
        {
            var user = await this._userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                return this.SetResultStatus<DerivedIdentityUser>(user, MessageStatus.Success, true);
            }
            else
            {
                return this.SetResultStatus<DerivedIdentityUser>(null, MessageStatus.Fail, false);
            }
        }


        private async Task<bool> isPasswordMatch(DerivedIdentityUser derivedIdentityUser, string Password)
        {
            var result = _userManager.PasswordHasher.VerifyHashedPassword(derivedIdentityUser, derivedIdentityUser.PasswordHash, Password);
            return (result == PasswordVerificationResult.Success);
        }
        private async Task<ServiceResponse<string>> SaveTokenCode(string code, Guid userID)
        {
            try
            {
                _unitOfWork.Instance.UserPasswordToken.Add(new UserPasswordToken()
                {
                    Code = code,
                    UserID = userID
                });
                var result = _unitOfWork.SaveChanges();

                if (result)
                    return SetResultStatus<string>(null, "password code saved successfully", true);
                else
                    return SetResultStatus<string>(null, "Unable to generate password.", false);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                return SetResultStatus<string>(null, Messages_Resources.Error, false);
            }
        }
        private async Task<ServiceResponse<string>> VerifyTokenCode(string code)
        {
            try
            {
                var result = _unitOfWork.Instance.UserPasswordToken.FirstOrDefault(x => x.Code == code);
                if (result != null)
                {
                    _unitOfWork.Instance.UserPasswordToken.Remove(result);
                    _unitOfWork.SaveChanges();
                    return SetResultStatus<string>(null, "password verified successfully", true);
                }
                else
                    return SetResultStatus<string>(null, "password is not verified", false);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                return SetResultStatus<string>(null, Messages_Resources.Error, false);
            }
        }


        public async Task<ServiceResponse<ForgotPasswordResponseModel>> ForgotPasswordLink(string username, string baseUrl)
        {
            var user = await this._userManager.FindByEmailAsync(username);
            if (user == null)
            {
                return SetResultStatus<ForgotPasswordResponseModel>(null, Messages_Resources.EmailNotRegisteredWithUs, false);
            }

            try
            {
                var code = await this._userManager.GeneratePasswordResetTokenAsync(user);
                await this.SaveTokenCode(code, user.Id);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = $"{baseUrl}Account/ResetPassword?code={code}&data={CipherHelper.Encrypt(user.Id.ToString())}";

                await _emailService.SendLinkForgotAsync(user.Email, callbackUrl);

                return SetResultStatus<ForgotPasswordResponseModel>(new ForgotPasswordResponseModel { CallbackURL = callbackUrl }, Messages_Resources.ResetPasswordSubmitted, true);

            }
            catch (Exception ex)
            {
                return SetResultStatus<ForgotPasswordResponseModel>(null, Messages_Resources.Error, false);
            }
        }

        public async Task<ServiceResponse<ForgotPasswordResponseModel>> CreatePasswordLink(string username, string baseUrl)
        {
            var user = await this._userManager.FindByEmailAsync(username);
            if (user == null)
            {
                return SetResultStatus<ForgotPasswordResponseModel>(null, Messages_Resources.EmailNotRegisteredWithUs, false);
            }

            try
            {
                var code = await this._userManager.GeneratePasswordResetTokenAsync(user);
                await this.SaveTokenCode(code, user.Id);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = $"{baseUrl}Account/CreatePassword?code={code}&data={CipherHelper.Encrypt(user.Id.ToString())}";

                await _emailService.SendLinkCreateAsync(user.Email, callbackUrl);

                return SetResultStatus<ForgotPasswordResponseModel>(new ForgotPasswordResponseModel { CallbackURL = callbackUrl }, Messages_Resources.ResetPasswordSubmitted, true);

            }
            catch (Exception ex)
            {
                return SetResultStatus<ForgotPasswordResponseModel>(null, Messages_Resources.Error, false);
            }
        }

        public async Task<ServiceResponse<ForgotPasswordResponseModel>> FoundMissingPetLink(string username, string baseUrl)
        {
            var user = await this._userManager.FindByEmailAsync(username);
            if (user == null)
            {
                return SetResultStatus<ForgotPasswordResponseModel>(null, Messages_Resources.EmailNotRegisteredWithUs, false);
            }

            try
            {
                var code = await this._userManager.GeneratePasswordResetTokenAsync(user);
                await this.SaveTokenCode(code, user.Id);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = $"{baseUrl}Account/CreatePassword?code={code}&data={CipherHelper.Encrypt(user.Id.ToString())}";

                await _emailService.SendLinkCreateAsync(user.Email, callbackUrl);

                return SetResultStatus<ForgotPasswordResponseModel>(new ForgotPasswordResponseModel { CallbackURL = callbackUrl }, Messages_Resources.ResetPasswordSubmitted, true);

            }
            catch (Exception ex)
            {
                return SetResultStatus<ForgotPasswordResponseModel>(null, Messages_Resources.Error, false);
            }
        }

        public async Task<ServiceResponse<ResetPasswordViewModel>> ResetPassword(ResetPasswordViewModel model)
        {
            DerivedIdentityUser user;
            try
            {
                var userID = CipherHelper.Decrypt(model.Data);
                user = await this._userManager.FindByIdAsync(userID);
            }
            catch
            {
                return SetResultStatus(model, Messages_Resources.InvalidRequest, false);
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code)).Replace(" ", "+");

            var result = await this._userManager.ResetPasswordAsync(user, code, model.Password);
            if (result.Succeeded)
            {
                return SetResultStatus(model, Messages_Resources.PasswordChangeSuccess, true);
            }
            else if ((await this.VerifyTokenCode(code)).IsSuccess)
            {
                var objRemovePwd = (await _userManager.RemovePasswordAsync(user));
                if (objRemovePwd.Succeeded)
                {
                    var objAddPwd = await _userManager.AddPasswordAsync(user, model.Password);
                    if (objAddPwd.Succeeded)
                    {
                        return SetResultStatus(model, Messages_Resources.PasswordChangeSuccess, true);
                    }
                }
                return SetResultStatus(model, Messages_Resources.ChangePasswordFailure, false);
            }
            else
            {
                string errorMessages = string.Join("</ br>", result.Errors.Select(x => x.Description));
                return SetResultStatus(model, errorMessages, false);
            }
        }

        public async Task<ServiceResponse<EnableAuthenticatorViewModel>> LoadSharedKeyAndQrCodeUriAsync(DerivedIdentityUser user, EnableAuthenticatorViewModel model)
        {
            ServiceResponse<EnableAuthenticatorViewModel> objReturn;
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user).WithCurrentCulture();
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user).WithCurrentCulture();
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user).WithCurrentCulture();
            }

            model.SharedKey = FormatKey(unformattedKey);
            model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);

            objReturn = this.SetResultStatus<EnableAuthenticatorViewModel>(model, MessageStatus.Success, true);
            return objReturn;
        }

        public async Task<ServiceResponse<string>> GetUserRole(string userName)
        {
            var user = await this._userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SetResultStatus(string.Empty, Messages_Resources.UserNotFound, false);
            }
            else
            {
                var roles = await this._userManager.GetRolesAsync(user);
                return SetResultStatus(roles.FirstNonDefault(), MessageStatus.Success, true);
            }
        }

        public async Task<ServiceResponse<DerivedIdentityUser>> GetUserById(string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return SetResultStatus<DerivedIdentityUser>(null, Messages_Resources.UserNotFound, false);
            }
            else
            {
                return SetResultStatus(user, MessageStatus.Success, true);
            }
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
            AuthenticatorUriFormat,
            _urlEncoder.Encode("TwoFactAuth"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }
        public async Task<ServiceResponse<RegisterViewModel>> CreateUserWithProfile(RegisterViewUserModel dto, Guid userId)
        {
            ServiceResponse<RegisterViewModel> objReturn;
            try
            {
                if (await this._userManager.FindByEmailAsync(dto.Email) != null)
                {
                    objReturn = this.SetResultStatus<RegisterViewModel>(null, Messages_Resources.EmailAlreadyExists, false);
                }
                //else if (!string.IsNullOrEmpty(dto.PhoneNumber) && await _unitOfWork.UserAccountRepository.isMobileNumberExists(dto.PhoneNumber))
                //{
                //    objReturn = this.SetResultStatus<RegisterViewModel>(null, Messages_Resources.PhoneAlreadyExists, false);
                //}
                else
                {
                    UserRegister user = new UserRegister
                    {
                        UserName = dto.Username,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Email = dto.Email,
                        Password = dto.Password,
                        Role = dto.Role,
                        Active = true,
                        CreatedBy = userId,
                        CreatedOn = DateTime.UtcNow,

                    };


                    var RegisteredUser = await _unitOfWork.UserAccountRepository.CreateUser(user);
                    await _unitOfWork.SaveChangesAsync();
                    //await _historyService.SaveUserProfileHistory(RegisteredUser, userId);

                    if (RegisteredUser.IsSuccess)
                    {
                        objReturn = this.SetResultStatus<RegisterViewModel>(null, MessageStatus.Success, true);
                    }
                    else
                    {

                        objReturn = this.SetResultStatus<RegisterViewModel>(null, RegisteredUser.error.Count > 0 ? RegisteredUser.error[1] : MessageStatus.Error, false);

                    }
                }
                return objReturn;
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<RegisterViewModel>(null, MessageStatus.Fail, false);
                return objReturn;
            }
        }
        public async Task<ServiceResponse<RegisterViewModel>> UpdateUser(RegisterViewUserModel dto)
        {
            ServiceResponse<RegisterViewModel> objReturn;
            UserRegister user = new UserRegister
            {
                Id = dto.Id,
                UserName = dto.Username,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role,
                Active = dto.IsActive
            };


            var RegisteredUser = await _unitOfWork.UserAccountRepository.UpdateUser(user);
            await _unitOfWork.SaveChangesAsync();
            if (RegisteredUser.IsSuccess)
            {
                objReturn = this.SetResultStatus<RegisterViewModel>(null, MessageStatus.Success, true);
            }
            else
            {

                objReturn = this.SetResultStatus<RegisterViewModel>(null, RegisteredUser.error.Count > 0 ? RegisteredUser.error[1] : MessageStatus.Error, false);

            }
            return objReturn;
        }
        public async Task<ServiceResponse<string>> SaveUploadedImagesAsync(Microsoft.AspNetCore.Http.IFormFile userImage, string foldername)
        {
            ServiceResponse<string> objReturn;
            try
            {


                if (userImage != null && userImage.ContentType.StartsWith("image/"))
                {
                    // Using GetValue<T> method
                    var webProjectRootPath = _configuration.GetValue<string>("WebProjectRootPath");

                    var uploads = Path.Combine(webProjectRootPath, "uploads", foldername);
                    //var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", foldername);
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var fileExtension = Path.GetExtension(userImage.FileName);
                    string fileName = $"{Path.GetFileNameWithoutExtension(userImage.FileName)}_{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploads, fileName);
                    string relativePath = Path.Combine("/uploads", foldername, fileName).Replace("\\", "/"); ;
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await userImage.CopyToAsync(fileStream);
                    }
                    objReturn = this.SetResultStatus<string>(relativePath, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<string>(null, MessageStatus.Fail, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<string>(null, MessageStatus.Error, false);
            }
            return objReturn;

        }
        public async Task<DerivedIdentityUser> FindUserByEmailID(string email)
        {
            try
            {
                return await this._userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                return null;
            }

        }
        public async Task<ServiceResponse<UserProfileViewModel>> GetProfile(Guid userId)
        {
            ServiceResponse<UserProfileViewModel> objReturn = new ServiceResponse<UserProfileViewModel>();
            try
            {
                UserProfileViewModel resDTO;
                var userRef = await this.GetUserById(userId.ToString());
                if (userRef == null) return this.SetResultStatus<UserProfileViewModel>(null, Messages_Resources.UserNotFound, false);


                var userProfile = this._unitOfWork.UserProfileRepository.GetUserProfile(userId);

                if (userProfile != null)
                {
                    resDTO = _mapper.Map<UserProfileViewModel>(userProfile);
                    resDTO.PhoneNumber = userRef.Data.PhoneNumber;
                    resDTO.Email = userRef.Data.Email;
                    resDTO.FirstName = userRef.Data.FirstName;
                    resDTO.LastName = userRef.Data.LastName;
                    resDTO.RoleName = (await this._userManager.GetRolesAsync(userRef.Data)).FirstNonDefault();
                    resDTO.Id = userRef.Data.Id;
                    resDTO.Username = userRef.Data.UserName;

                    objReturn = this.SetResultStatus<UserProfileViewModel>(resDTO, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<UserProfileViewModel>(null, MessageStatus.ProfileNotFound, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<UserProfileViewModel>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }

        public async Task<ServiceResponse<RegisterViewUserModel>> GetProfileDetails(string id)
        {
            var response = new ServiceResponse<RegisterViewUserModel>();
            try
            {
                var userRef = await this._userManager.FindByIdAsync(id);
                var role = await this._userManager.GetRolesAsync(userRef);

                if (userRef != null)
                {
                    RegisterViewUserModel user = new RegisterViewUserModel();
                    var userProfiles = (await this.GetUserProfileByUserID(Guid.Parse(id))).Data;
                    user.Id = userRef.Id;
                    user.Email = userRef.Email;
                    user.FirstName = userRef.FirstName;
                    user.LastName = userRef.LastName;
                    user.Password = userRef.PasswordHash;
                    user.IsActive = userRef.IsActive ?? false;
                    user.Username = userRef.UserName;
                    user.IsDeviceConnected = userRef.IsDeviceConnected ?? false;
                    user.TwoFactorEnabled = userRef.TwoFactorEnabled;
                    user.Role = role.First();
                    user.IsProfile = false;
                    if (userProfiles != null)
                    {
                    }

                    response = SetResultStatus<RegisterViewUserModel>(user, Messages_Resources.Success, true);
                }
                else
                {
                    response = SetResultStatus<RegisterViewUserModel>(null, MessageStatus.Error, false);
                }

            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                response = SetResultStatus<RegisterViewUserModel>(null, MessageStatus.Error, false);
            }
            return response;
        }

        public async Task<ServiceResponse<UserProfile>> GetUserProfileByUserID(Guid userId)
        {
            ServiceResponse<UserProfile> objReturn;
            try
            {
                var profile = this._unitOfWork.UserProfileRepository.Get(x => x.UserId == userId).FirstOrDefault();
                objReturn = this.SetResultStatus<UserProfile>(profile, profile != null ? MessageStatus.Success : MessageStatus.ProfileNotFound, (profile != null));

            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<UserProfile>(null, MessageStatus.Fail, false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<List<UserDetailModel>>> GetUsers(UserListFilterModel requestParam)
        {
            var objReturn = new ServiceResponse<List<UserDetailModel>>();
            try
            {
                requestParam.search = requestParam.search?.Trim();
                var usersList = _unitOfWork.UserAccountRepository.GetUsers().AsQueryable();

                var totalRecord = usersList.Count();
                var response = usersList.OrderByDescending(x => x.CreatedOn).Skip(requestParam.start).Take(requestParam.length).ToList();
                (objReturn.Data, objReturn.recordsTotal, objReturn.recordsFiltered, objReturn.IsSuccess) = (response, totalRecord, totalRecord, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<List<UserDetailModel>>(null, MessageStatus.Fail, true);
            }

            return objReturn;
        }

        public EnumUserType GetUserType(Guid userid)
        {
            try
            {
                var user = _unitOfWork.UserAccountRepository.GetUsers().FirstOrDefault(a=>a.Id == userid);
                return user != null ? user.UserType : EnumUserType.User;
            }
            catch (Exception ex)
            {
                return EnumUserType.User;
            }
        }

        public async Task<ServiceResponse<DerivedIdentityUser>> DeleteUser(string id)
        {
            var objectReturn = new ServiceResponse<DerivedIdentityUser>();
            try
            {
                var response = await _unitOfWork.UserAccountRepository.DeleteUser(id);
                if (response != null)
                    objectReturn = SetResultStatus<DerivedIdentityUser>(response, MessageStatus.Delete, true);
                else
                    objectReturn = SetResultStatus<DerivedIdentityUser>(null, MessageStatus.NotFound, false);
            }
            catch (Exception)
            {
                objectReturn = SetResultStatus<DerivedIdentityUser>(null, MessageStatus.Error, false);
            }
            return objectReturn;
        }

        public async Task<ServiceResponse<DerivedIdentityUser>> DeleteUserPermanent(string id)
        {
            var objectReturn = new ServiceResponse<DerivedIdentityUser>();
            try
            {

                var response = await _unitOfWork.UserAccountRepository.DeleteUserPermanent(id);
                if (response != null)
                {
                    DeleteUserOnRecharge(response.ShopifyId);
                    var user = _unitOfWork.UserAccountRepository.GetUsers().FirstOrDefault(a => a.ShopifyId == response.ShopifyId);
                    petService.DeleteAllPets(user.Id, null);
                    subscriptionService.DeleteAllSubscriptions(response.ShopifyId);

                    objectReturn = SetResultStatus<DerivedIdentityUser>(response, MessageStatus.Delete, true);
                }
                else
                    objectReturn = SetResultStatus<DerivedIdentityUser>(null, MessageStatus.NotFound, false);
            }
            catch (Exception)
            {
                objectReturn = SetResultStatus<DerivedIdentityUser>(null, MessageStatus.Error, false);
            }
            return objectReturn;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task LogOutAsync(DerivedIdentityUser user, HttpContext context)
        {
            await _signInManager.SignOutAsync();
            await _userManager.UpdateSecurityStampAsync(user);
            await context.SignOutAsync(IdentityConstants.ApplicationScheme);
        }
        public async Task<ServiceResponse<UserProfileViewModel>> GetUserByReferralCode(string referral)
        {
            ServiceResponse<UserProfileViewModel> objReturn;

            try
            {
                //var referralcode = this._unitOfWork.UserProfileRepository.Find(x => x.ReferralCode == referral).FirstOrDefault();
                var referralcode = this._unitOfWork.UserProfileRepository.FirstOrDefault();
                if (referralcode is null)
                {
                    return this.SetResultStatus<UserProfileViewModel>(null, MessageStatus.NotExists, false);
                }
                objReturn = this.SetResultStatus<UserProfileViewModel>(new UserProfileViewModel()
                {
                    UserId = referralcode.UserId.Value,
                    Id = referralcode.Id,
                    //ReferralCode = referralcode.ReferralCode

                }, MessageStatus.Success, true);

            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<UserProfileViewModel>(null, MessageStatus.Fail, false);
            }
            return objReturn;

        }

        public async Task<ServiceResponse<IList<UserViewModel>>> GetUserByRole(string Role)
        {
            var response = new ServiceResponse<IList<UserViewModel>>();
            try
            {
                var userProfiles = (from individualUser in _unitOfWork.Instance.Users.IgnoreQueryFilters()
                                    join userrole in _unitOfWork.Instance.UserRoles on individualUser.Id equals userrole.UserId
                                    join role in _unitOfWork.Instance.Roles on userrole.RoleId equals role.Id

                                    where role.Name.Contains(Role) && individualUser.IsDeleted != true
                                    orderby individualUser.Id descending
                                    select new UserViewModel
                                    {
                                        UserName = individualUser.UserName,
                                        FirstName = individualUser.FirstName,
                                        LastName = individualUser.LastName,
                                        Email = individualUser.Email,
                                        PhoneNumber = !string.IsNullOrEmpty(individualUser.PhoneNumber) ? individualUser.PhoneNumber : string.Empty,
                                        IsActive = individualUser.IsActive ?? false,
                                        Id = individualUser.Id,
                                        Enc_Id = EncDec.Encrypt(Convert.ToString(individualUser.Id), string.Empty),
                                        StrCreatedOn = individualUser.CreatedOn.ToString("dd/MM/yyyy"),
                                    }).ToList();

                response = SetResultStatus<IList<UserViewModel>>(userProfiles, Messages_Resources.Success, true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                response = SetResultStatus<IList<UserViewModel>>(null, MessageStatus.Error, false);
            }
            return response;
        }

        public async Task<ServiceResponse<RegisterViewModel>> CreateUser(RegisterViewModel dto)
        {
            ServiceResponse<RegisterViewModel> objReturn;
            try
            {
                if (await this._userManager.FindByEmailAsync(dto.Username) != null)
                {
                    return this.SetResultStatus<RegisterViewModel>(null, Messages_Resources.UsernameAlreadyExists, false);
                }

                UserRegister user = new UserRegister
                {
                    UserName = dto.Username,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    Password = dto.Password,
                    Role = dto.Role,
                    Active = true,
                    CreatedBy = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    CustomerGuid = Guid.NewGuid(),
                    MobileCountryCode = dto.MobileCountryCode,
                };

                var RegisteredUser = await _unitOfWork.UserAccountRepository.CreateUser(user);

                await _historyService.SaveUserHistory(RegisteredUser);
                var registerDTO = _mapper.Map<RegisterViewModel>(dto);

                if (RegisteredUser.IsSuccess)
                {
                    objReturn = this.SetResultStatus<RegisterViewModel>(registerDTO, MessageStatus.Success, true);
                }
                else
                {

                    objReturn = this.SetResultStatus<RegisterViewModel>(null, MessageStatus.Error, false);

                }

                return objReturn;
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<RegisterViewModel>(null, MessageStatus.Fail, false);
                return objReturn;
            }
        }

        /// <summary>
        /// Add Secondary User With It's Parent ID
        /// </summary>
        /// <param name="secondaryUser"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> AddSecondaryUser(SecondaryUserDTO secondaryUser, Guid parentId)
        {
            ServiceResponse<string> objReturn;
            try
            {

                DerivedIdentityUser existingTempUser;
                DerivedIdentityUser existingUser;
                existingTempUser = this._unitOfWork.Instance.Users.FirstOrDefault(x => x.PhoneNumber == secondaryUser.PhoneNumber);

                if (existingTempUser is not null)
                    return this.SetResultStatus<string>(null, Messages_Resources.PhoneAlreadyExists, false);

                existingUser = await this._userManager.FindByEmailAsync(secondaryUser.Email);
                if (existingUser is not null)
                    return this.SetResultStatus<string>(null, Messages_Resources.EmailAlreadyRegisteredWithUs, false);

                var userName = secondaryUser.Email;
                var password = "Test@123";
                var userModel = new UserRegister
                {
                    FirstName = secondaryUser.FirstName,
                    LastName = secondaryUser.LastName,
                    UserName = userName,
                    Email = secondaryUser.Email,
                    Password = password,
                    Active = true,
                    PhoneNumber = secondaryUser.PhoneNumber,
                    CreatedOn = DateTime.UtcNow,
                    CustomerGuid = Guid.NewGuid(),
                    Role = EnumRole.SecondayUser.ToString(),
                    MobileCountryCode = 0,
                    ParentUserID = parentId
                };

                var RegisteredUser = await _unitOfWork.UserAccountRepository.CreateAccount(userModel);
                await _historyService.SaveUserHistory(RegisteredUser);

                if (RegisteredUser.IsSuccess)
                {
                    await _emailService.SendSecondaryUserEmailAsync(userModel.Email, password);

                    //Send mail to user with content of Username and Password
                    objReturn = this.SetResultStatus<string>("Success", MessageStatus.SecondaryUserAdded, true);
                }
                else
                {
                    if (RegisteredUser.error.Count > 0)
                        objReturn = this.SetResultStatus<string>(null, RegisteredUser.error[1], false);
                    else
                        objReturn = this.SetResultStatus<string>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<string>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }

        /// <summary>
        /// Create ID Check user for Guest User
        /// </summary>
        /// <param name="secondaryUser"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<GUIDViewModel>> CreateIDCheckUser(FoundMissingPetRequest foundMissingPetRequest)
        {
            ServiceResponse<GUIDViewModel> objReturn;
            try
            {

                DerivedIdentityUser existingTempUser;
                DerivedIdentityUser existingUser;

                var guidModel = new GUIDViewModel();


                existingTempUser = this._unitOfWork.Instance.Users.FirstOrDefault(x => x.PhoneNumber == foundMissingPetRequest.ContactNumber.ToString());

                if (existingTempUser is not null)
                {
                    guidModel.UserID = existingTempUser.Id;
                    return this.SetResultStatus<GUIDViewModel>(guidModel, MessageStatus.Success, true);
                }

                existingUser = await this._userManager.FindByEmailAsync(foundMissingPetRequest.Email);
                if (existingUser is not null)
                {
                    guidModel.UserID = existingUser.Id;
                    return this.SetResultStatus<GUIDViewModel>(guidModel, MessageStatus.Success, true);
                }


                var userModel = new UserRegister
                {
                    FirstName = foundMissingPetRequest.FirstName,
                    LastName = foundMissingPetRequest.LastName,
                    Email = foundMissingPetRequest.Email,
                    UserName = foundMissingPetRequest.Email,
                    Password = "Test@123",
                    Active = true,
                    PhoneNumber = foundMissingPetRequest.ContactNumber,
                    CreatedOn = DateTime.UtcNow,
                    CustomerGuid = Guid.NewGuid(),
                    Role = EnumRole.AnonymousUser.ToString(),
                    MobileCountryCode = 0,
                };



                var RegisteredUser = await _unitOfWork.UserAccountRepository.CreateAccount(userModel);
                await _historyService.SaveUserHistory(RegisteredUser);

                if (RegisteredUser.IsSuccess)
                {
                    //Send mail to user with content of Username and Password

                    guidModel.UserID = RegisteredUser.Id;
                    objReturn = this.SetResultStatus<GUIDViewModel>(guidModel, MessageStatus.Success, true);
                }
                else
                {
                    if (RegisteredUser.error.Count > 0)
                        objReturn = this.SetResultStatus<GUIDViewModel>(null, RegisteredUser.error[1], false);
                    else
                        objReturn = this.SetResultStatus<GUIDViewModel>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<GUIDViewModel>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }


        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<DerivedIdentityUser>> UpdateProfile(EditUserRequestViewModel dto, Guid userID)
        {
            // Initialize the response object
            var objReturn = new ServiceResponse<DerivedIdentityUser>();

            // Find the user by ID
            var user = await _userManager.FindByIdAsync(userID.ToString());

            if (user == null)
            {
                objReturn = this.SetResultStatus<DerivedIdentityUser>(null, MessageStatus.UserNotFound, false);
                return objReturn;
            }

            // Update user properties
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.ImagePath = dto.UserImage;
            user.Address = dto.Address;
            user.LicenseNumber = dto.LicenseNumber;
            user.IssuingAuthority = dto.IssuingAuthority;

            // Save changes
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                objReturn = this.SetResultStatus<DerivedIdentityUser>(user, MessageStatus.ProfileUpdated, true);

            }
            else
            {
                var errorMessage = result.Errors.FirstOrDefault()?.Description ?? MessageStatus.Error;
                objReturn = this.SetResultStatus<DerivedIdentityUser>(null, errorMessage, false);
            }

            return objReturn;
        }

        public async Task<string> DeleteUserOnRecharge(int customerId)
        {
            try
            {
                var rechargeUrl = _configuration.GetValue<string>("CustomKeys:RechargeUrl");
                var rechargeKey = _configuration.GetValue<string>("CustomKeys:RechargeApiKey");
                var rechargeVersion = _configuration.GetValue<string>("CustomKeys:RechargeApiVersion");
                using var form = new MultipartFormDataContent();

                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(rechargeUrl)
                };
                httpClient.DefaultRequestHeaders.Add("X-Recharge-Access-Token", rechargeKey);
                httpClient.DefaultRequestHeaders.Add("X-Recharge-Version", rechargeVersion);

                var response = await httpClient.DeleteAsync($"customers/" + customerId);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

    }
}
