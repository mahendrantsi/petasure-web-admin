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
    public class HomeController : Controller
    {
        public HomeController() 
        {
        }

        public IActionResult Error(string id)
        {
            ViewBag.ErrorCode = id;
            return View();
        }

    }
}
