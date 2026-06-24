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

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CheckUser")]
    public class SubscriptionController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IMapper _mapper;
        private readonly ISettingService _Service;
        private readonly ISubscriptionService _subscriptionService;
        private readonly UserManager<DerivedIdentityUser> _userManager;
        private readonly string environment;

        public SubscriptionController(IAccountService accountService, IJwtAuthManager jwtAuthManager, IMapper mapper, 
            ISettingService service, UserManager<DerivedIdentityUser> userManager, ISubscriptionService subscriptionService)
        {
            this._accountService = accountService;
            this._jwtAuthManager = jwtAuthManager;
            this._mapper = mapper;
            this._Service = service;
            this._userManager = userManager;
            _subscriptionService = subscriptionService;
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            environment = configuration["CustomKeys:Environment"];
        }

        /// <summary>
        /// Get Pet Info for current Login User
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetActiveSubscriptions")]
        public async Task<IActionResult> GetActiveSubscriptions(Guid userId)
        {

            var response = await _subscriptionService.GetActiveSubscriptions(userId);

            return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);

        }

        [HttpGet("GetActiveSubscriptionList")]
        public async Task<IActionResult> GetActiveSubscriptionList(Guid userId)
        {

            var response = await _subscriptionService.GetActiveSubscriptionList(userId, _accountService.GetUserType(userId));

            return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);

        }

        [HttpPost("SaveInAppPurchase")]
        public async Task<IActionResult> SaveInAppPurchase(InAppPurchaseInputViewModel model)
        {
            if (model.AspnetuserId == Guid.Empty || model.AspnetuserId == null)
                model.AspnetuserId = base.GetCurrentUserId();
            var response = await _subscriptionService.SaveInAppPurchase(model);

            return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);

        }
        
        [HttpPost("IsCertificateValid")]
        public async Task<IActionResult> IsCertificateValid(Guid userid, bool isSandBox = false)
        {
            var response = await _subscriptionService.IsCertificateValid(userid, isSandBox);

            return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);

        }

    }
}