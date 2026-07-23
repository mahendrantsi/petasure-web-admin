using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Project.Core.Enum;
using Project.Core.Extension;
using Project.Models.AccountModel;
using Project.Web.Resources;
using Project.Models.AccountModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using NToastNotify;
using Project.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Project.Data.ExtendedDBEntities;
using System.Security.Claims;
using Project.Services.Service;
using Project.Models.CommonModel;
using Project.Core.ActionFilter;
using Project.Models.ProfileModel;
using Project.Web.Common;
using AutoMapper;

namespace Project.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IToastNotification toastNotification;
        private readonly IAccountService accountService;
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;
        private readonly SignInManager<DerivedIdentityUser> _signInManager;
        private readonly UserManager<DerivedIdentityUser> _userManager;
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
        public AccountController(IToastNotification objToastNotification, IAccountService objAccountService, UserManager<DerivedIdentityUser> userManager, SignInManager<DerivedIdentityUser> signInManager, ISettingService settingService, IMapper mapper)
        {
            this.toastNotification = objToastNotification;
            this.accountService = objAccountService;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._settingService = settingService;
            this._mapper = mapper;
        }
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return this.View(model);
        }


        [HttpGet]
        public async Task<IActionResult> RegisterAsync(string referral)
        {
            var model = new RegisterViewModel();

            await accountService.GetUserByReferralCode(referral).ContinueWith(x =>
            {
                if (x.Result.IsSuccess)
                {
                    model.ReferralCode = referral;
                    ViewBag.ReferralCode = referral;
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var returnUrl = loginViewModel.ReturnUrl;
            try
            {
                RemoveLoginValidation();
                if (!this.ModelState.IsValid)
                {
                    String messages = String.Join(Environment.NewLine, this.ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception));
                    this.toastNotification.AddAlertToastMessage(messages);
                }
                loginViewModel.Email = CommonHelper.TrimMobileNo(loginViewModel.Email);
                var serviceResponse = await this.accountService.Login(new LoginReqDTO() { Username = loginViewModel.Email, Password = loginViewModel.Password }, false);
                if (serviceResponse.Data is null)
                {
                    this.toastNotification.AddErrorToastMessage($"Invalid username/phone number or password");
                    return this.View(loginViewModel);
                }

                var objRole = this.accountService.GetUserRole(serviceResponse.Data.User.UserName).Result;
                await SetClaims(serviceResponse.Data.User, objRole.Data);

                Enum[] accessRole = { EnumRole.Admin, EnumRole.User };
                Enum.TryParse(objRole.Data, out EnumRole myStatus);
                if (accessRole.Contains(myStatus))
                {
                    if (serviceResponse.IsSuccess)
                    {
                        return RedirectToAction("RedirectUserWithRuleCheck", new { userId = serviceResponse.Data.User.Id.ToString(), returnUrl });
                    }
                    else if (serviceResponse.Message == MessageStatus.RequiresTwoFactor)
                    {
                        return RedirectToAction(nameof(LoginWith2fa), new { returnUrl });
                    }
                    else if (serviceResponse.Message == MessageStatus.EnableAuthenticator)
                    {
                        return RedirectToAction(nameof(EnableAuthenticator), new { userId = serviceResponse.Data.User.Id.ToString() });
                    }
                    else if (serviceResponse.Message == MessageStatus.InactiveAccount)
                    {
                        this.toastNotification.AddErrorToastMessage(serviceResponse.Message);
                    }
                    else
                    {
                        this.toastNotification.AddErrorToastMessage(Error_Resources.Invalid_User);
                    }
                }
                else
                {
                    return RedirectToAction("RedirectUserWithRuleCheck", new { userId = serviceResponse.Data.User.Id.ToString(), returnUrl });
                }

            }
            catch (Exception exception)
            {
                this.toastNotification.AddErrorToastMessage(Error_Resources.Error_500);
            }
            return this.View(loginViewModel);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var forgot = await accountService.ForgotPasswordLink(model.EMail, Project.Web.Common.ConfigurationManager.GetBaseUrl());

                if (forgot.IsSuccess)
                {
                    TempData["message"] = "A link has been sent your email address";
                    return RedirectToAction("Thanks");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string code, string data)
        {
            var model = new ResetPasswordViewModel
            {
                Code = code,
                Data = data,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                var response = await accountService.ResetPassword(resetPassword);
                if (response.IsSuccess)
                {
                    TempData["message"] = "Password was reset successfully";
                    return RedirectToAction("Thanks");
                }
                else
                {
                    return Json(new { isValid = false, message = response.Message });
                }
            }
            return this.View(resetPassword);
        }

        [HttpGet]
        public IActionResult CreatePassword(string code, string data)
        {
            var model = new ResetPasswordViewModel
            {
                Code = code,
                Data = data,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePassword(ResetPasswordViewModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                var response = await accountService.ResetPassword(resetPassword);
                if (response.IsSuccess)
                {
                    TempData["message"] = "Password successfully created";
                    return RedirectToAction("Thanks");
                }
                else
                {
                    return Json(new { isValid = false, message = response.Message });
                }
            }
            return this.View(resetPassword);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Thanks()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        #region PRIVATE METHODS


        /// <summary>
        /// Remove Model validation for  FCMToken DeviceType
        /// </summary>
        private void RemoveLoginValidation()
        {
            ModelState.Remove("FCMToken");
            ModelState.Remove("DeviceType");
        }
        private void RemoveRegisterValidation()
        {
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("PhoneNumber");
        }
        #endregion

        [Authorize]
        private async Task SetClaims(DerivedIdentityUser user, string objRole)
        {
            var identity = Request.HttpContext.User;
            var identitys = identity.Identity as ClaimsIdentity;

            //var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            //var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if (!string.IsNullOrEmpty(objRole))
            {
                var existingRoleClaims = await _userManager.GetClaimsAsync(user);

                if (existingRoleClaims == null || existingRoleClaims.Count == 0)
                {
                    await _userManager.AddClaimAsync(user, new Claim("Role", objRole));
                }
                else
                {
                    var roleClaim = existingRoleClaims.FirstOrDefault(c => c.Type == "Role" && c.Value == objRole);
                    if (roleClaim == null)
                    {
                        await _userManager.AddClaimAsync(user, new Claim("Role", objRole));
                    }

                }

            }

        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator(string userId)
        {
            //var id = Convert.ToInt64(Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userId.ToString())).Replace(" ", "+"));
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentNullException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new EnableAuthenticatorViewModel();
            model.UserId = userId;
            var resQr = await accountService.LoadSharedKeyAndQrCodeUriAsync(user, model);

            return View(resQr.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                throw new ArgumentNullException("User not found.");
            }

            if (!ModelState.IsValid)
            {
                var resQr = await accountService.LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(resQr.Data);
            }

            // STRIP SPACES AND HYPENS
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);


            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Code", "Verification code is invalid.");
                var resQr = await accountService.LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(resQr.Data);
            }
            else
            {
                await _signInManager.TwoFactorAuthenticatorSignInAsync(verificationCode, false, false);
            }

            //UPDATE ISDEVICECONNECTED PROPERTY
            user.IsDeviceConnected = true;
            await _userManager.UpdateAsync(user);

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

            return RedirectToAction("RedirectUserWithRuleCheck", new { userId = user.Id.ToString() });
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe)
        {
            // Ensure the user has gone through the username & password screen first

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ArgumentNullException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel();
            ViewData["ReturnUrl"] = "";

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ArgumentNullException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, model.TwoFactorCode);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                return RedirectToAction("RedirectUserWithRuleCheck", new { userId = user.Id.ToString() });
            }
            else if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }
        public async Task<IActionResult> RedirectUserWithRuleCheck(string userId, string returnUrl = null)
        {
            var serviceResponse = await this.accountService.GetUserById(userId);
            if (serviceResponse.IsSuccess)
            {
                var objRole = this.accountService.GetUserRole(serviceResponse.Data.UserName).Result;
                if (objRole.Data is null)
                {
                    this.toastNotification.AddErrorToastMessage(Error_Resources.Error_500);
                }
                var enumCheck = Enum.TryParse(objRole.Data, out EnumRole outRole);
                if (enumCheck)
                {
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    switch (outRole)
                    {
                        case EnumRole.SubAdmin:
                        case EnumRole.Admin:
                            return this.RedirectToAction("Index", "dashboard", new { area = EnumRole.Admin.ToString() });
                            break;

                        case EnumRole.AnonymousUser:
                            return this.RedirectToAction("Index", "AnonymousUser", new { area = EnumRole.Admin.ToString() });
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    this.toastNotification.AddErrorToastMessage($"unable to file role of user");
                    return RedirectToAction("Login");
                }
            }
            else
            {
                this.toastNotification.AddErrorToastMessage(Error_Resources.Invalid_User);
            }
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> LogOut()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) await accountService.LogOutAsync(user, this.HttpContext);

            return RedirectToAction("Login", "Account");
        }
    }
}
