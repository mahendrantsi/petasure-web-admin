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

        /// <summary>
        /// Maps a recognition-scan AI response (raw JSON string shaped like
        /// {success, status, message, data:{...}}) to the matching outer HTTP status code.
        /// Previously every recognition endpoint (Similar/AnalyzeDog/Register/RegisterCat)
        /// always returned this.Ok(...) regardless of what the AI decided, so a hard
        /// validation rejection (not-a-pet / wrong-species) — signaled by the AI via an
        /// embedded "status": 201 and "success": false — was invisible to any caller that
        /// checks the transport-level HTTP status instead of deserializing the body.
        /// </summary>
        protected IActionResult RecognitionScanResult(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return this.Ok(response);
            }

            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(response);
                var root = doc.RootElement;
                var status = root.TryGetProperty("status", out var statusProp) && statusProp.TryGetInt32(out var statusVal)
                    ? statusVal
                    : 200;
                return this.StatusCode(status, response);
            }
            catch (System.Text.Json.JsonException)
            {
                // Unexpected/non-JSON body — fall back to the previous behaviour rather than fail the request.
                return this.Ok(response);
            }
        }
    }
}