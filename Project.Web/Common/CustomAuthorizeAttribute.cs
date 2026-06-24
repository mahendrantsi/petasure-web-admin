using Project.Core.Enum;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Project.Web.Common
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext authorizationFilterContext)
        {
            var url = authorizationFilterContext.HttpContext.Request.Path.Value;
            if (!authorizationFilterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                authorizationFilterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            PreventUnAuthorizeUser(authorizationFilterContext, url);

        }

        private void PreventUnAuthorizeUser(AuthorizationFilterContext authorizationFilterContext, string url)
        {
            if (!url.Contains("Logout"))
            {
                if (this.allowedroles != null)
                {
                    var user = authorizationFilterContext.HttpContext.User;
                    int count = 0;
                    foreach (var role in this.allowedroles)
                    {
                        var res = user.IsInRole(role);
                        if (res)
                        {
                            count += 1;
                        }
                    }
                    if (this.allowedroles.Count() != 0 && count == 0)
                    {
                        authorizationFilterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {  controller = "Account", action = "AccessDenied" }));
                    }
                }
            }
        }
    }


    public class CustomRequiredAttribute : ValidationAttribute
    {
        public CustomRequiredAttribute(string propertyName)
        {
            OtherProperty = propertyName;
        }
        public string OtherProperty { get; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var role = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty).GetValue(validationContext.ObjectInstance);
            if (EnumRole.User == (EnumRole)role && value == null)
            {
                return new ValidationResult("The " + validationContext.DisplayName + " Field is required");
            }
            else
                return ValidationResult.Success;
        }
    }
}
