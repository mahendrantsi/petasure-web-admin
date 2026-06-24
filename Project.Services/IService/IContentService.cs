namespace Project.Services.IService
{
    using System.Collections.Generic; 
    using AspNetCore.ServiceRegistration.Dynamic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Project.Models.CommonModel; 
    using Project.Services.ServiceEntities;  
    using System.Threading.Tasks;
    using Project.Data.DBEntities;
    using Project.Models.Content;
    using System;

    [ScopedService]
    public interface IContentService
    {
        Task<ServiceResponse<List<ContentViewModel>>> GetContentList(JQueryDataTableModel param);
        Task<ServiceResponse<List<ContentViewModel>>> GetContentList();
        ServiceResponse<Project.Models.Content.ContentViewModel> GetContentWithUrl(string name);
        ServiceResponse<ContentViewModel> GetContent(Guid id);
        Task<ServiceResponse<ContentInfo>> Edit(Project.Models.Content.ContentViewModel model);
        Task<ServiceResponse<ContentInfo>> Add(Project.Models.Content.ContentViewModel model);
        Task<ServiceResponse<ContectusViewModel>> ReadEnquiry(ContectusViewModel model,int readBy);
        Task<ServiceResponse<ContectusViewModel>> GetEnquiryByID(Guid ID);
        Task<ServiceResponse<List<ContectusViewModel>>> GetEnquiryList(JQueryDataTableModel requestParam, string type = "");
        Task<ServiceResponse<ContectusViewModel>> AddEnquiry(ContactUsRequestViewModel model, Guid userId);
        //Task<ServiceResponse<AccountDeactivationViewModel>> DeActivateAccountRequest(AccountDeactivationViewModel model, string baseUrl = "");
        Task<ServiceResponse<EnqViewModel>> SubmitEnquiryResponse(EnqViewModel model);
     //   Task<ServiceResponse<List<EnquiryResponseViewModel>>> GetEnquiryResponseList(Guid enquiryID);
        Task<ServiceResponse<string>> CheckAccountDeactivationByUserId(Guid userId);

        Task<ServiceResponse<List<FaqViewModel>>> GetFaq();
    }
}
