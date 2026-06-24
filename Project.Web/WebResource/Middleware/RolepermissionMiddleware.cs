using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Project.Core.Enum;
using Project.Services.Service;
using Project.Services.IService;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Project.Data.ExtendedDBEntities;
using Microsoft.AspNetCore.Identity;

namespace Project.Web.WebResource.Middleware
{
    public class RolepermissionMiddleware
    {
        private readonly RequestDelegate next;
     
        //private readonly SignInManager<DerivedIdentityUser> _signInManager;
        //private readonly IRolePremissionServices rolePremissionServices;

        public RolepermissionMiddleware(RequestDelegate next)//, SignInManager<DerivedIdentityUser> signInManager)
        {
            this.next = next;
            //_signInManager = signInManager;
            //this.rolePremissionServices = rolePremissionServices;

        }
        public async Task Invoke(HttpContext httpContext, ProjectDbContext context)
        {
            //if (httpContext.User.Identity.IsAuthenticated)
            //{

            //    //var claimsPrincipal = httpContext.User;
            //    var identity = httpContext.User.Identity as ClaimsIdentity;
            //    var uniqueClaims = identity.Claims
            //          .GroupBy(c => new { c.Type, c.Value })
            //          .Select(g => g.First())
            //          .ToList();
            //    var endpoint = httpContext.GetEndpoint();
            //    if (endpoint != null)
            //    {
            //        if (endpoint.Metadata.GetMetadata<ControllerActionDescriptor>() is ControllerActionDescriptor actionDescriptor)
            //        {
            //            var actionName = actionDescriptor.ActionName;
            //            var methodName = actionDescriptor.MethodInfo.Name;
            //            var displayNameAttribute = actionDescriptor.MethodInfo.GetCustomAttribute<DisplayNameAttribute>();
            //            bool displayNameMatched = false;
            //            if (displayNameAttribute != null)
            //            {
            //                string displayName = displayNameAttribute.DisplayName;
            //                foreach (var claim in uniqueClaims)
            //                {
            //                    if (displayName != claim.Value)
            //                    {
            //                        displayNameMatched = true;
            //                        break;
            //                        //httpContext.Response.Redirect("/Home/NotFound");
            //                        //return;
            //                    }
            //                }
            //                if (!displayNameMatched)
            //                {
            //                    httpContext.Response.Redirect("/Home/NotFound");
            //                    return;
            //                }
            //            }

            //            // var attribute = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;

            //            //}

            //        }
            //    }
            //}

            await next(httpContext);
        }
        


    }
}
