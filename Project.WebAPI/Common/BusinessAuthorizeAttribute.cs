using Project.Core.Enum;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http.Features;
using System.Net;

namespace Project.WebAPI.Common
{
    public class BusinessAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] allowedroles;
        public BusinessAuthorizeAttribute()
        {
            //this.allowedroles = roles;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
        {
            if (filterContext != null)
            {
                Microsoft.Extensions.Primitives.StringValues authTokens;
                filterContext.HttpContext.Request.Headers.TryGetValue("authToken", out authTokens);

                var _token = authTokens.FirstOrDefault();

                if (_token != null)
                {
                    string authToken = _token;
                    if (authToken != null)
                    {
                        if (IsValidToken(authToken))
                        {
                            filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
                            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
                            filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");
                            return;
                        }
                        else
                        {
                            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                            filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide Auth Token";
                            filterContext.Result = new JsonResult("Please Provide Auth Token")
                            {
                                Value = new
                                {
                                    Status = "Error",
                                    Message = "Please Provide Auth Token"
                                },
                            };

                        }
                    }
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide Auth Token";
                    filterContext.Result = new JsonResult("Please Provide Auth Token")
                    {
                        Value = new
                        {
                            Status = "Error",
                            Message = "Please Provide Auth Token"
                        },
                    };
                }
            }
        }

        private bool IsValidToken(string authToken)
        {
            return true;
        }
    }
}
