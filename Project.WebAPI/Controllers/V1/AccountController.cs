using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;


namespace Project.WebAPI.Controllers.V1
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Project.Core.Enum;
    using Project.Core.Extension;
    using Project.Data;
    using Project.Data.ExtendedDBEntities;
    using Project.Logger;
    using Project.Models.AccountModel;
    using Project.Models.CommonModel;
    using Project.Models.ProfileModel;
    using Project.Services.IService;
    using Project.Services.Service;
    using Project.WebAPI.APIResource;
    using Project.WebAPI.Common;
    using Project.WebAPI.Helpers;
    using Project.WebAPI.Infrastructure;
    using Project.WebAPI.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using AutoMapper;
    using System.Linq;
    using Project.Models.AdminModel;
    using Project.Models.User;
    using System.Data;
    using Project.Models.AdminRule;
    using Microsoft.AspNetCore.Identity;
    using Project.Services.ServiceEntities;
    using System.Collections.Generic;
    using System.Data.Entity.SqlServer.Utilities;
    using System.Security.Cryptography;
    using System.Globalization;
    using Project.Services.Utilities;
    using Newtonsoft;
    using Newtonsoft.Json;
    using Project.Middleware;
    using Microsoft.AspNetCore.WebUtilities;
    using System.Text;
    using Project.Data.DBEntities;
    using ServiceStack.Script;
    using Project.Core.ActionFilter;
    using System.IO;
    using Project.Models.GeneralModel;

    /// <summary>
    /// Account related API endpoints (registration, login, profile, password and token management).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CheckUser")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ISettingService _Service;
        private readonly UserManager<DerivedIdentityUser> _userManager;
        private readonly string environment;
        private readonly IPetService _petService;

        /// <summary>
        /// Initializes a new instance of <see cref="AccountController"/>.
        /// </summary>
        public AccountController(IAccountService accountService, IJwtAuthManager jwtAuthManager, IMapper mapper, 
            IUserService userService, ISettingService service, UserManager<DerivedIdentityUser> userManager, IPetService petService)
        {
            this._accountService = accountService;
            this._jwtAuthManager = jwtAuthManager;
            this._mapper = mapper;
            this._userService = userService;
            this._Service = service;
            this._userManager = userManager;

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            environment = configuration["CustomKeys:Environment"];
            _petService = petService;
        }

        /// <summary>
        /// Registers a new in-app customer.
        /// </summary>
        /// <param name="dto">Registration DTO.</param>
        /// <returns>201/200 on success, 400 on validation failure.</returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        [TrimStringProperties]
        public async Task<IActionResult> RegisterCustomerAsync([FromBody] RegisterCustomerReqDTO dto)
        {
            var model = _mapper.Map<RegisterViewModel>(dto);
            model.UserType = EnumUserType.InApp;
            var serviceResponse = await this._accountService.RegisterCustomer(model);
            return serviceResponse.IsSuccess switch { true => this.Ok(serviceResponse), _ => this.BadRequest(serviceResponse) };
        }

                
        /// <summary>
        /// Performs user login and returns tokens / MFA flow if required.
        /// </summary>
        /// <param name="dto">Login request DTO.</param>
        /// <returns>JWT tokens on success, Unauthorized / NotFound otherwise.</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginReqDTO dto)
        {
            var serviceResponse = await this._accountService.Login(dto);

            if (serviceResponse.IsSuccess)
            {
                var objResultToken = await this.GetClaimsIdentity(serviceResponse.Data.User);
                if (objResultToken is null)
                {
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Message = "Token not generated.";
                    return this.BadRequest(serviceResponse);
                }
                else
                {
                    serviceResponse.Data.Tokens = objResultToken; 
                    return this.Ok(serviceResponse);
                }
            }
            else if (!serviceResponse.IsSuccess && serviceResponse.Data?.User?.TwoFactorEnabled is true)
            {
                if (serviceResponse.Data.User.IsDeviceConnected is not true)
                {
                    await _userManager.ResetAuthenticatorKeyAsync(serviceResponse.Data.User).WithCurrentCulture();
                    var keyResponse = await _accountService.LoadSharedKeyAndQrCodeUriAsync(serviceResponse.Data.User, new EnableAuthenticatorViewModel()).WithCurrentCulture();
                    serviceResponse.Data.MFAKey = keyResponse.Data?.SharedKey;
                }
                return this.Unauthorized(serviceResponse);
            }
            else if (!serviceResponse.IsSuccess && serviceResponse.Data is not null)
                return this.Unauthorized(new { serviceResponse.Message, serviceResponse.IsSuccess, data = (object)null });
            else
                return this.NotFound(serviceResponse);
        }

        /// <summary>
        /// Logs user out and removes refresh tokens.
        /// </summary>
        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            var response = new ResponseData();
            await this._accountService.UpdateDeviceDetails(base.GetCurrentUserId(), null, null);
            this._jwtAuthManager.RemoveRefreshTokenByUserName(User.Identity.Name);

            var user = await _userManager.GetUserAsync(User);
            if (user != null) await _accountService.LogOutAsync(user, this.HttpContext);


            response.coreRes = new APICoreRes { status = APICoreRes.ResStatus.Success, userMessage = "Logged Out Successfully." };
            response.Data = null;
            return this.Ok(response);
        }

        /// <summary>
        /// Refresh access token using refresh token.
        /// </summary>
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                    return BadRequest(new { Message = "Refresh token is required." });

                var jwtResult = this._jwtAuthManager.Refresh(request.RefreshToken, request.AccessToken, DateTime.Now);

                return this.Ok(new Models.LoginResult
                {
                    UserName = jwtResult.UserName,
                    Role = this.User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString,
                });
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        /// <summary>
        /// Initiates forgot-password flow by sending a reset link.
        /// </summary>
        /// <param name="email">User email address.</param>
        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var serviceResponse = await this._accountService.ForgotPasswordLink(email, Project.WebAPI.Common.ConfigurationManager.GetBaseUrl());
            return serviceResponse.IsSuccess ? this.Ok(serviceResponse) : this.BadRequest(serviceResponse);
        }

        /// <summary>
        /// Change password for the authenticated user.
        /// </summary>
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            var serviceResponse = await this._accountService.ChangePassword(base.GetCurrentUserId(), dto.OldPassword, dto.NewPassword);
            return serviceResponse.IsSuccess ? this.Ok(serviceResponse) : this.BadRequest(serviceResponse);
        }

        /// <summary>
        /// Permanently delete the current user.
        /// </summary>
        [HttpGet("Delete")]
        public async Task<IActionResult> Delete()
        {
            var serviceResponse = await this._accountService.DeleteUserPermanent(base.GetCurrentUserId().ToString());
            return serviceResponse.IsSuccess ? this.Ok(serviceResponse) : this.BadRequest(serviceResponse);
        }

        #region Private methods
        /// <summary>
        /// Creates JWT tokens for the given user.
        /// </summary>
        /// <param name="derivedIdentityUser">Target identity user.</param>
        /// <returns>JWT auth result containing access and refresh tokens.</returns>
        private async Task<JwtAuthResult> GetClaimsIdentity(DerivedIdentityUser derivedIdentityUser)
        {

            var claims = new[]{
                new Claim(ClaimTypes.Name, derivedIdentityUser.UserName),
                new Claim(ClaimTypes.NameIdentifier, derivedIdentityUser.Id.ToString()),
            };
            return this._jwtAuthManager.GenerateTokens(derivedIdentityUser.UserName, claims, DateTime.Now);


        }
        #endregion

        /// <summary>
        /// Adds a secondary user for the current account.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("AddSecondaryUser")]
        [TrimStringProperties]
        public async Task<IActionResult> AddSecondaryUserAsync([FromBody] SecondaryUserDTO dto)
        {
            var serviceResponse = await this._accountService.AddSecondaryUser(dto, base.GetCurrentUserId());
            return serviceResponse.IsSuccess switch { true => this.Ok(serviceResponse), _ => this.BadRequest(serviceResponse) };
        }

        /// <summary>
        /// Gets profile details for the current logged in user.
        /// </summary>
        [HttpGet("GetProfileDetail")]
        public async Task<IActionResult> GetProfileDetail()
        {

            var response = await _accountService.GetUserDetailById(base.GetCurrentUserId());

            return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);

        }

        /// <summary>
        /// Updates the current user's profile.
        /// </summary>
        [HttpPost("UpdateProfile")]
        [TrimStringProperties]
        public async Task<IActionResult> EditUserAsync([FromBody] EditUserRequestViewModel dto)
        {
            var serviceResponse = await this._accountService.UpdateProfile(dto, base.GetCurrentUserId());
            return serviceResponse.IsSuccess switch { true => this.Ok(serviceResponse), _ => this.BadRequest(serviceResponse) };
        }

        /// <summary>
        /// Uploads a user image file.
        /// </summary>
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(UploadImageRequestViewModel dto)
        {
            var serviceResponse = await this._accountService.SaveUploadedImagesAsync(dto.Image, dto.FolderName);
            return serviceResponse.IsSuccess switch { true => this.Ok(serviceResponse), _ => this.BadRequest(serviceResponse) };
        }

    }
}