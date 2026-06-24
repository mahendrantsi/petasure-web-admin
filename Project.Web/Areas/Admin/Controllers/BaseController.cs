// <copyright file="BaseController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Project.Web.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;
    using Project.Models.CommonModel;

    [Area("admin")]
    public class BaseController : Controller
    {
        protected Guid GetCurrentUserId()
        {
            var claimsPrincipal = this.User as ClaimsPrincipal;
            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            return Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        protected string GetCurrentUserName()
        {
            var claimsPrincipal = this.User as ClaimsPrincipal;
            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            return claimsIdentity.FindFirst(ClaimTypes.Name).Value;
        }

        protected string GetCurrentUserRole()
        {
            var claimsPrincipal = this.User as ClaimsPrincipal;
            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            return claimsIdentity.FindFirst(ClaimTypes.Role).Value;
        }

        protected virtual JQueryDataTableModel GetPageData()
        {
            var pstart = HttpContext.Request.Form["start"].FirstOrDefault();
            var plength = Request.Form["length"].FirstOrDefault();
            var pordercolumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"] + "][name]"];
            var psortorder = HttpContext.Request.Form["order[0][dir]"];
            var psearch = HttpContext.Request.Form["search[value]"];

            if (string.IsNullOrEmpty(psortorder))
            {
                psortorder = "asc";
            }

            JQueryDataTableModel jQueryDataTableModel = new JQueryDataTableModel
            {
                length = !string.IsNullOrEmpty(plength) ? Convert.ToInt32(plength) : 10,
                start = !string.IsNullOrEmpty(pstart) ? Convert.ToInt32(pstart) : 0,
                ordercolumn = pordercolumn,
                sortorder = psortorder,
                search = psearch,
            };

            return jQueryDataTableModel;
        }
    }
}
