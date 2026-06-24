//  <copyright file="BaseController.cs" company="PlaceholderCompany">
//  Copyright (c) PlaceholderCompany. All rights reserved.
//  </copyright>

namespace Project.WebAPI.Controllers.V1
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Castle.Components.DictionaryAdapter;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : ControllerBase
    {
        protected string GetModelStateError(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            
             var err=   string.Join(", ", modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            return err;
        }

        protected Guid GetCurrentUserId()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            return new Guid(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        protected string GetCurrentUserName()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            return claimsIdentity.FindFirst(ClaimTypes.Name).Value;
        }

        protected string GetCurrentUserRole()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            return claimsIdentity.FindFirst(ClaimTypes.Role).Value;
        }

        protected Guid GetBusinessID()
        {
            var context = HttpContext.Request.Headers.Where(x => x.Key == "authToken").FirstOrDefault();
            return new Guid(context.Value);
        }
    }
}