using Microsoft.AspNetCore.Mvc;
using Project.Core.ActionFilter;
using Project.Models.CommonModel;
using Project.Services.Service;
using Project.Services.ServiceEntities;
using System.Threading.Tasks;
using System;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using NToastNotify;
using Project.Services.IService;
using Project.Web.Common;
using Project.Web.Areas.Admin.Controllers;
using Project.Core.Enum;
using Project.Core.Extension;
using System.Collections.Generic;
using System.Linq;
using Project;
using Project.Web;
using Project.Web.Controllers;
using Humanizer;
using Project.Models.ProfileModel;

namespace Project.Web.Areas.Admin.Controllers
{
    public class UserManagementController : BaseController
    {
        private readonly IUserService _userRoleManagementService;
        private readonly IToastNotification toastNotification;
        private readonly IDataProtector _protector;
        private readonly IAccountService accountService;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IMapper _mapper;
        private readonly IMissingService _missingService;

        public UserManagementController(IUserService userRoleManagementService, IToastNotification objToastNotification, 
            IDataProtectionProvider provider, IAccountService objAccountService, IExceptionLoggerService exceptionLoggerService, IMapper mapper, IMissingService missingService)
        {
            _userRoleManagementService = userRoleManagementService;
            toastNotification = objToastNotification;
            accountService = objAccountService;
            _protector = provider.CreateProtector("Project.UserRoleManagement");
            _exceptionLoggerService = exceptionLoggerService;
            _mapper = mapper;
            _missingService = missingService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AppUsers()
        {
            return View();
        }
        public async Task<IActionResult> Users(UserListFilterModel searchRequest)
        {
            try
            {
                var pstart = HttpContext.Request.Form["start"].FirstOrDefault();
                var plength = Request.Form["length"].FirstOrDefault();
                var pordercolumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"] + "][name]"];
                var psortorder = HttpContext.Request.Form["order[0][dir]"];

                if (string.IsNullOrEmpty(pordercolumn))
                    pordercolumn = "CreatedOn";


                if (string.IsNullOrEmpty(psortorder))
                    psortorder = "desc";

                searchRequest.length = !string.IsNullOrEmpty(plength) ? Convert.ToInt32(plength) : 15;
                searchRequest.start = !string.IsNullOrEmpty(pstart) ? Convert.ToInt32(pstart) : 0;
                searchRequest.ordercolumn = pordercolumn;
                searchRequest.sortorder = psortorder;


                var result = await accountService.GetUsers(searchRequest);
                return new JsonResult(new { result.recordsFiltered, result.recordsTotal, data = result.Data });

            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                return new JsonResult(new { recordsFiltered = 0, recordsTotal = 0, data = (object)null });
            }
        }

        [HttpGet]
        public async Task<IActionResult> User(string id)
        {
                var response = await accountService.GetUserById(id);
                return Json(response.Data);
        }

        [HttpPost]
        [TrimStringProperties]
        public async Task<IActionResult> User(RegisterViewUserModel model)
        {
            try
            {
                ViewBag.User = model.Role;
                if (model.Id == Guid.Empty)
                {
                    if (ModelState.IsValid)
                    {
                        ServiceResponse<RegisterViewModel> serviceResponse;
                        serviceResponse = await accountService.CreateUserWithProfile(model, GetCurrentUserId());
                        if (serviceResponse.IsSuccess)
                        {
                            toastNotification.AddSuccessToastMessage($"{model.Role.ToString()} Added Successfully");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            toastNotification.AddErrorToastMessage(serviceResponse.Message);
                            return View(model);
                        }
                    }
                    else
                    {
                        toastNotification.AddErrorToastMessage("Incorrect Details");
                        return View(model);
                    }
                }
                else
                {
                    if (!model.IsChangePassword)
                    {
                        RemovePasswordValidation();
                    }

                    if (ModelState.IsValid)
                    {
                        ServiceResponse<RegisterViewModel> serviceResponse;
                        serviceResponse = await accountService.UpdateUser(model);
                        if (serviceResponse.IsSuccess)
                        {
                            toastNotification.AddSuccessToastMessage($"{model.Role.ToString()} Updated Successfully");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            toastNotification.AddErrorToastMessage(serviceResponse.Message);
                            return View(model);
                        }
                    }
                    else
                    {
                        toastNotification.AddErrorToastMessage("Incorrect Details");
                        return View(model);
                    }
                }
            }
            catch (Exception)
            {
                toastNotification.AddErrorToastMessage($"{model.Role.ToString()} operation failed.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var response = await accountService.DeleteUser(Id);
            if (response.IsSuccess)
                toastNotification.AddSuccessToastMessage($"User Deleted Successfully");
            else
                toastNotification.AddSuccessToastMessage($"{response.Message}");
            return RedirectToAction("SubUser");
        }



        /// <summary>
        /// CreateSubUser Method.
        /// </summary>
        /// <returns>CreateSubUser View.</returns>
        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        public async Task<IActionResult> SubUser()
        {
            var serviceResponse = await accountService.GetUserByRole(EnumRole.SubAdmin.ToString());
            return View(serviceResponse.Data);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSubUser()
        {
           
            return View();
        }

        [HttpPost]
        [TrimStringProperties]
        [CustomAuthorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSubUser(RegisterViewModel model)
        {
            try
            {
                ModelState.Remove("TermsConditions");
                ModelState.Remove("Username");
                ModelState.Remove("MobileCountryCode");
                if (ModelState.IsValid)
                {
                    model.Username = model.Email;
                    ServiceResponse<RegisterViewModel> serviceResponse;
                    serviceResponse = await accountService.CreateUser(model);
                    if (serviceResponse.IsSuccess)
                    {
                        this.toastNotification.AddSuccessToastMessage("User Added Successfully");
                        return RedirectToAction("SubUser");
                    }
                    else
                    {
                        this.toastNotification.AddErrorToastMessage(serviceResponse.Message);
                        return View(model);
                    }
                }
                else
                {
                    this.toastNotification.AddErrorToastMessage("Incorrect Details");
                    return View(model);
                }
            }
            catch (Exception)
            {
                this.toastNotification.AddErrorToastMessage("User registration failed.");
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        public async Task<IActionResult> EditSubUser(string Id)
        {
            var dt = await accountService.GetUserById(Id);
            if(dt.Data!= null)
            {
                var model = new RegisterViewModel()
                {
                    Username = dt.Data.UserName,
                    Email = dt.Data.Email,
                    FirstName = dt.Data.FirstName,
                    LastName = dt.Data.LastName,
                    PhoneNumber = dt.Data.PhoneNumber,
                    IsActive = dt.Data.IsActive.Value
                };
                return View(model);
            }
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [TrimStringProperties]
        [CustomAuthorize(Roles = "Admin")]
        public async Task<IActionResult> EditSubUser(RegisterViewUserModel model)
        {
            try
            {
                ModelState.Remove("TermsConditions");
                if (!model.IsChangePassword)
                {
                    RemovePasswordValidation();
                }
                if (ModelState.IsValid)
                { 

                    var  serviceResponse = await accountService.UpdateUser(model);

                    if (serviceResponse.IsSuccess)
                    {
                        this.toastNotification.AddSuccessToastMessage("User Updated Successfully");
                        return RedirectToAction("SubUser");
                    }
                    else
                    {
                        this.toastNotification.AddErrorToastMessage("Failed to Update User!");
                        return View(model);
                    }
                }
                else
                {
                    this.toastNotification.AddErrorToastMessage("Incorrect Details");
                    return View(model);
                }
            }
            catch (Exception)
            {
                this.toastNotification.AddErrorToastMessage("User Update failed.");
                return View(model);
            }
        }
        private void RemovePasswordValidation()
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
        }

        /// <summary>
        /// CreateSubUser Method.
        /// </summary>
        /// <returns>CreateSubUser View.</returns>
        [HttpGet]
        [CustomAuthorize(Roles = "Admin,SubAdmin")]
        public async Task<IActionResult> IDCheckUser()
        {
            var serviceResponse = await _missingService.AdminIDCheckPets();
            return View(serviceResponse.Data);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Admin,SubAdmin")]
        public async Task<IActionResult> Profile()
        {
            var serviceResponse = await accountService.GetProfileDetails(base.GetCurrentUserId().ToString());
            return View(serviceResponse.Data);
        }
    }
}
