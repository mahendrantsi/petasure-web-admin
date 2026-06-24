using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Azure;
using NToastNotify;
using Project.Core.Enum;
using Project.Models.CommonModel;
using Project.Models.Content;
using Project.Models.Master;
using Project.Services.IService;
using Project.Services.Service;
using Project.Web.Common;
using Project.Web.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ContentManagementController : BaseController
    {
        private readonly IToastNotification toastNotification;
        private readonly IContentService contentService;
        public ContentManagementController(IToastNotification objToastNotification, IContentService _contentService)
        {
            this.toastNotification = objToastNotification;
            this.contentService = _contentService;
        }

        [CustomAuthorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var response = await this.contentService.GetContentList(new JQueryDataTableModel() { ordercolumn = "Id" });
            return View(response.Data);
        }

        [CustomAuthorize(Roles = "Admin")]
        public IActionResult ContentLayout(Guid? id)
        {
            if (id is not null)
                return View(this.contentService.GetContent(id.Value).Data);
            else
                return View(new ContentViewModel() { });
        }

        [HttpPost]
        public async Task<IActionResult> AddEditContent(ContentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                this.toastNotification.AddErrorToastMessage(Error_Resources.Error_500);
                return View(model);
            }

            model.CreatedBy = base.GetCurrentUserId();


            var response = await ((model.Id is null) ? this.contentService.Add(model) : this.contentService.Edit(model));
            if (response.IsSuccess)
            {
                this.toastNotification.AddSuccessToastMessage($"page saved successfully");
            }
            else
            {
                this.toastNotification.AddErrorToastMessage(response.Message);
            }

            return this.RedirectToAction("Index", "ContentManagement");
        }

        [HttpPost]
        public async Task<JsonResult> UploadTempDocFile(IFormFile file)
        {
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\layoutData", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return Json(new { IsSuccess = true, fileName = file.FileName });
            }
            else
            {
                return Json(new { IsSuccess = false, errormessgae = "No file available " });
            }
        }

        [CustomAuthorize(Roles = "Admin,SubAdmin,AnonymousUser")]
        public async Task<IActionResult> Enquiry(string type)
        {
            Enum.TryParse(type, out EnumEnquiryViewType myStatus);
            ViewBag.myStatus = myStatus;
            return View();
        }

        [CustomAuthorize(Roles = "Admin,SubAdmin")]
        public async Task<IActionResult> GetEnquiryList(string type)
        {
            try
            {
                var pstart = HttpContext.Request.Form["start"].FirstOrDefault();
                var plength = Request.Form["length"].FirstOrDefault();
                var pordercolumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"] + "][name]"];
                var psortorder = HttpContext.Request.Form["order[0][dir]"];
                var psearch = HttpContext.Request.Form["search[value]"];

                pordercolumn = "ID";


                if (string.IsNullOrEmpty(psortorder))
                {
                    psortorder = "desc";
                }


                JQueryDataTableModel jQueryDataTableModel = new JQueryDataTableModel
                {
                    length = !string.IsNullOrEmpty(plength) ? Convert.ToInt32(plength) : 15,
                    start = !string.IsNullOrEmpty(pstart) ? Convert.ToInt32(pstart) : 0,
                    ordercolumn = pordercolumn,
                    sortorder = psortorder,
                    search = psearch,
                };

                var result = await contentService.GetEnquiryList(jQueryDataTableModel, type);
                return new JsonResult(new { recordsFiltered = result.recordsFiltered, recordsTotal = result.recordsTotal, data = result.Data.OrderByDescending(a=>a.Createdon) });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { recordsFiltered = 0, recordsTotal = 0, data = (object)null });
            }
        }

        public async Task<IActionResult> GetEnquiry(Guid id)
        {
            var result = await contentService.GetEnquiryByID(id);
            if (result.IsSuccess)
            {
                //if (result.Data.EnquiryType == EnumHelper.GetEnumDescription(EnumEnquiryType.Enquiry))
                //{
                //    await contentService.ReadEnquiry(new ContectusViewModel() { ID = id }, Convert.ToInt32(base.GetCurrentUserId()));
                //}
            }
            return PartialView("_EnquiryView", result.Data);
        }

        public async Task<IActionResult> GetEnquiryRequest(Guid id)
        {
            var result = await contentService.GetEnquiryByID(id);
            //if (result.IsSuccess)
            //{
            //    if (result.Data.EnquiryType == EnumHelper.GetEnumDescription(EnumEnquiryType.Enquiry))
            //    {
            //        await contentService.ReadEnquiry(new ContectusViewModel() { ID = id }, Convert.ToInt32(base.GetCurrentUserId()));
            //    }
            //}
            return PartialView("_EnquiryViewNew", result.Data);
        }

        public async Task<ActionResult> SubmitEnquiryResponse(EnqViewModel model)
        {
            model.UserID = base.GetCurrentUserId();
            return Json(await contentService.SubmitEnquiryResponse(model));
        }

        //public async Task<IActionResult> GetEnquiryResponseList(long enquiryID)
        //{
        //    var result = await contentService.GetEnquiryResponseList(enquiryID);
        //    return PartialView("_EnquiryResponseList", result.Data);
        //}        

    }


}
