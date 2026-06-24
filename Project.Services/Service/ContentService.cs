namespace Project.Services.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Project.Core.Enum;
    using Project.Core.Extension;
    using Project.Data;
    using Project.Data.DBEntities;
    using Project.Models.CommonModel;
    using Project.Services.IService;
    using Project.Services.ServiceEntities;
    using Project.Persistence.UOW;
    using Project.Data.ExtendedDBEntities;
    using Microsoft.AspNetCore.DataProtection;
    using System.Data.Entity.SqlServer;
    using Project.Models.Content;
    using ServiceStack;
    using System.Globalization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.WebUtilities;
    using System.Text;
    using System.ComponentModel;
    using Project.Models.CommonModel;
    using Project.Services.ServiceEntities;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Project.Core;

    public class ContentService : BaseService, IContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IAccountService accountService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IHistoryService _historyService;
        private readonly UserManager<DerivedIdentityUser> _userManager;


        public ContentService(IUnitOfWork unitOfWork, IExceptionLoggerService exceptionLoggerService, IAccountService accountService, IEmailService emailService, IMapper mapper, IHistoryService historyService, UserManager<DerivedIdentityUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._exceptionLoggerService = exceptionLoggerService;
            this.accountService = accountService;
            _emailService = emailService;
            _historyService = historyService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<List<ContentViewModel>>> GetContentList(JQueryDataTableModel param)
        {
            ServiceResponse<List<ContentViewModel>> objReturn = new ServiceResponse<List<ContentViewModel>>();
            try
            {
                var propertyInfo = typeof(ContentViewModel).GetProperty(param.ordercolumn);
                var dd = _unitOfWork.Instance.ContentInfo.ToList();
                var contentResponse = _unitOfWork.Instance.ContentInfo.Select(content => new ContentViewModel()
                {
                    Id = content.Id,
                    Content = content.Content,
                    Description = content.Description,
                    Name = content.Name,
                    IsActive = content.IsActive,
                    ModifyOn = content.ModifiedOn.Value
                }).ToList();

                if (contentResponse.Count > 0)
                {
                    objReturn = this.SetResultStatus<List<ContentViewModel>>(contentResponse, MessageStatus.Success, true);
                    (contentResponse, objReturn.recordsTotal, objReturn.recordsFiltered) = DataTableShorting<ContentViewModel>(contentResponse, param, propertyInfo);
                }
                else
                {
                    objReturn = this.SetResultStatus<List<ContentViewModel>>(new List<ContentViewModel>(), MessageStatus.NotExists, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<List<ContentViewModel>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<List<ContentViewModel>>> GetContentList()
        {
            ServiceResponse<List<ContentViewModel>> objReturn = new ServiceResponse<List<ContentViewModel>>();
            try
            {
               
                var dd = _unitOfWork.Instance.ContentInfo.ToList();
                var contentResponse = _unitOfWork.Instance.ContentInfo.Select(content => new ContentViewModel()
                {
                    Id = content.Id,
                    Content = content.Content,
                    Description = content.Description,
                    Name = content.Name,
                    IsActive = content.IsActive,
                    ModifyOn = content.ModifiedOn.Value
                }).ToList();

                if (contentResponse.Count > 0)
                {
                    objReturn = this.SetResultStatus<List<ContentViewModel>>(contentResponse, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<List<ContentViewModel>>(new List<ContentViewModel>(), MessageStatus.NotExists, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<List<ContentViewModel>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        public ServiceResponse<Project.Models.Content.ContentViewModel> GetContent(Guid id)
        {
            ServiceResponse<Project.Models.Content.ContentViewModel> objReturn = new ServiceResponse<Project.Models.Content.ContentViewModel>();
            try
            {
                var contentQuery = this._unitOfWork.Instance.ContentInfo.Where(x => x.Id == id).Select(x => new Project.Models.Content.ContentViewModel()
                {
                    Content = x.Content,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    ModifyBy = x.ModifiedBy.Value,
                    ModifyOn = x.ModifiedOn.Value
                });

                objReturn = this.AutoSetResult<Project.Models.Content.ContentViewModel>(contentQuery.FirstOrDefault(), MessageStatus.Success);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<Project.Models.Content.ContentViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }


        public ServiceResponse<Project.Models.Content.ContentViewModel> GetContentWithUrl(string name)
        {
            ServiceResponse<Project.Models.Content.ContentViewModel> objReturn = new ServiceResponse<Project.Models.Content.ContentViewModel>();
            try
            {
                ContentViewModel contentResponse = this._unitOfWork.Instance.ContentInfo
                    .Where(x => x.Url.ToLower() == name.ToLower() && x.IsActive)
                    .Select(x => new Project.Models.Content.ContentViewModel()
                    {
                        Content = x.Content,
                        Name = x.Name,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        ModifyBy = x.ModifiedBy.Value,
                        ModifyOn = x.ModifiedOn.Value

                    }).FirstOrDefault();

                objReturn = this.SetResultStatus<Project.Models.Content.ContentViewModel>(contentResponse, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<Project.Models.Content.ContentViewModel>(null, MessageStatus.Error, false);
            }
            return objReturn;
        }


        public async Task<ServiceResponse<ContentInfo>> Edit(Project.Models.Content.ContentViewModel model)
        {
            ServiceResponse<ContentInfo> objReturn = new ServiceResponse<ContentInfo>();
            try
            {
                var content = this._unitOfWork.Instance.ContentInfo.FirstOrDefault(x => x.Id == model.Id);

                if (content != null)
                {
                    content.ModifiedBy = model.ModifyBy;
                    content.ModifiedOn = DateTime.UtcNow;
                    content.Content = model.Content;
                    content.Name = model.Name;
                    content.Description = model.Description;
                    content.IsActive = model.IsActive;
                    content.Url = model.Name.Trim().Replace(" ", "-");
                    _unitOfWork.GenericRepository<ContentInfo>().UpdateEntity(content);
                    await this._unitOfWork.SaveChangesAsync();
                    objReturn = this.SetResultStatus<ContentInfo>(content, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<ContentInfo>(content, MessageStatus.NotExists, true);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<ContentInfo>(null, MessageStatus.SomethingWentWrong, false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<ContentInfo>> Add(Project.Models.Content.ContentViewModel model)
        {
            ServiceResponse<ContentInfo> objReturn = new ServiceResponse<ContentInfo>();
            try
            {
                var content = new ContentInfo()
                {
                    CreatedBy = model.CreatedBy,
                    ModifiedBy = model.ModifyBy,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    Content = model.Content,
                    Name = model.Name,
                    Description = model.Description,
                    IsActive = model.IsActive,
                    Url = model.Name.Trim().Replace(" ", "-")
                };
                _unitOfWork.GenericRepository<ContentInfo>().Add(content);
                await this._unitOfWork.SaveChangesAsync();
                objReturn = this.SetResultStatus<ContentInfo>(content, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<ContentInfo>(null, MessageStatus.SomethingWentWrong, false);
            }

            return objReturn;
        }


        public async Task<ServiceResponse<ContectusViewModel>> AddEnquiry(ContactUsRequestViewModel model, Guid userId)
        {
            ServiceResponse<ContectusViewModel> objReturn = new ServiceResponse<ContectusViewModel>();
            try
            {

                //Get User Data by userID
                var user = await _userManager.FindByIdAsync(userId.ToString());


                var fullName = user.FirstName + " " + user.LastName;
                var content = new Enquiry()
                {
                    CreatedOn = DateTime.UtcNow,
                    Subject = model.Subject,
                    Message = model.Message,
                    FullName = fullName,
                    PhoneNo = user.PhoneNumber,
                    Email = user.Email,
                    Status = EnumEnquiryStatus.Open
                };

                _unitOfWork.GenericRepository<Enquiry>().Add(content);
                await this._unitOfWork.SaveChangesAsync();
                //.ContinueWith((o) =>
                //{
                //    _emailService.EnquiryNotificationNew(new EnquiryNotificationRequest()
                //    {
                //        Date = content.CreatedOn.GetDateTimeStringWithTime(),
                //        Email = content.Email,
                //        UserName = content.FullName,
                //        EnquiryID = content.EnquiryCode.ToString(),
                //        Status = content.Status,
                //    });

                //});
                objReturn = this.SetResultStatus<ContectusViewModel>(null, MessageStatus.ContactUsSuccessMessage, true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<ContectusViewModel>(null, MessageStatus.SomethingWentWrong, false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<EnqViewModel>> SubmitEnquiryResponse(EnqViewModel model)
        {
            try
            {
                var enquiry = _unitOfWork.Instance.Enquiry.FirstOrDefault(x => x.Id == model.EnquiryID);
                if (enquiry is null)
                {
                    this.SetResultStatus<EnqViewModel>(null, MessageStatus.NotExists, false);
                }
                else
                {
                    enquiry.Status = model.Status;
                    _unitOfWork.GenericRepository<Enquiry>().UpdateEntity(enquiry);

                    _unitOfWork.GenericRepository<EnquiryResponse>().Add(new EnquiryResponse()
                    {
                        Answer = model.Answer,
                        PlainAnswer = model.PlainAnswer,
                        CreatedBy = model.UserID,
                        EnquiryID = model.EnquiryID,
                        Status = model.Status,
                        SendMail = model.SendMail,
                    });



                    await this._unitOfWork.SaveChangesAsync();
                }

                return this.SetResultStatus<EnqViewModel>(model, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                return this.SetResultStatus<EnqViewModel>(null, MessageStatus.SomethingWentWrong, false);
            }

        }

        public async Task<ServiceResponse<List<ContectusViewModel>>> GetEnquiryList(JQueryDataTableModel requestParam, string type = "")
        {
            ServiceResponse<List<ContectusViewModel>> objReturn = new ServiceResponse<List<ContectusViewModel>>();
            try
            {
                DateTime? date = null;

                try
                {
                    date = DateTime.ParseExact(requestParam.search.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                }
                catch
                {
                    // HANDLE DATE EXCEPTION
                }

                Enum.TryParse(type, out EnumEnquiryViewType tabType);
                var enqCode = Guid.TryParse(requestParam.search,out Guid enqCodeExact);
                var parsingStatusEnum = Enum.TryParse(typeof(EnumEnquiryStatus), requestParam.search, out object? enumValue);
                var parsingTypeEnum = Enum.TryParse(typeof(EnumEnquiryType), requestParam.search.Replace(" ", ""), out object? enumTypeValue);

                var details = _unitOfWork.Instance.Enquiry.Where(x =>

                ((tabType == EnumEnquiryViewType.All) || (tabType == EnumEnquiryViewType.Enquiry && x.EnquiryType == EnumEnquiryType.Enquiry) || (tabType == EnumEnquiryViewType.Deactivation && x.EnquiryType == EnumEnquiryType.DeactivationRequest)) &&
                (string.IsNullOrEmpty(requestParam.search) || (!string.IsNullOrEmpty(requestParam.search) && (x.Email.Contains(requestParam.search)
                                                                                                           || x.FullName.Contains(requestParam.search)
                                                                                                           || x.PhoneNo.Contains(requestParam.search))
                                                                                                           || (enqCode && x.EnquiryCode == enqCodeExact)
                                                                                                           || (date != null && x.CreatedOn.Date == date.Value.Date)
                                                                                                           || (parsingStatusEnum && (EnumEnquiryStatus)enumValue == x.Status)
                                                                                                           || (parsingTypeEnum && (EnumEnquiryType)enumTypeValue == x.EnquiryType)))


                ).Select(x => new ContectusViewModel
                {
                    Name = x.FullName,
                    Subject = x.Subject,
                    Description = x.Message,
                    ID = x.Id,
                    Email = x.Email,
                    PhoneNo = x.PhoneNo,
                    status = x.Status,
                    SendOn =  x.CreatedOn.GetScriptDateDMYStr(),
                    EnquiryType = EnumHelper.GetEnumDescription(x.EnquiryType),
                    EnquiryCode = x.EnquiryCode,
                    Createdon   = x.CreatedOn
                }).OrderByDescending(m => m.Createdon).ToList();

                var propertyInfo = typeof(ContectusViewModel).GetProperty(requestParam.ordercolumn);
                objReturn = this.SetResultStatus<List<ContectusViewModel>>(details, MessageStatus.Success, true);
                (objReturn.Data, objReturn.recordsTotal, objReturn.recordsFiltered) = DataTableShorting<ContectusViewModel>(details, requestParam, propertyInfo);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<List<ContectusViewModel>>(null, MessageStatus.SomethingWentWrong, false);
            }

            return objReturn;
        }

        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public async Task<ServiceResponse<ContectusViewModel>> GetEnquiryByID(Guid ID)
        {
            ServiceResponse<ContectusViewModel> objReturn = new ServiceResponse<ContectusViewModel>();
            try
            {

                var details = _unitOfWork.Instance.Enquiry.Where(x => x.Id == ID).Select(x => new ContectusViewModel
                {
                    Name = x.FullName,
                    Description = x.Message,
                    ID = x.Id,
                    Email = x.Email,
                    PhoneNo = x.PhoneNo,
                    status = x.Status,
                    SendOn = x.CreatedOn.GetDateTimeStringWithTime(),
                    UserId = x.UserId,
                    EnquiryType = EnumHelper.GetEnumDescription(x.EnquiryType).ToString(),

                }).FirstOrDefault();

                objReturn = this.SetResultStatus<ContectusViewModel>(details, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<ContectusViewModel>(null, MessageStatus.SomethingWentWrong, false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<ContectusViewModel>> ReadEnquiry(ContectusViewModel model, int readBy)
        {
            ServiceResponse<ContectusViewModel> objReturn = new ServiceResponse<ContectusViewModel>();
            try
            {
                var enquiry = _unitOfWork.GenericRepository<Enquiry>().Get(x => x.Id == model.ID).FirstOrDefault();
                if (enquiry is not null)
                {
                    enquiry.Status = EnumEnquiryStatus.Read;
                    enquiry.ReadOn = DateExtension.GetUtcDateTime;
                    enquiry.ReadBy = readBy;
                    _unitOfWork.GenericRepository<Enquiry>().UpdateEntity(enquiry);
                    await this._unitOfWork.SaveChangesAsync();
                    objReturn = this.SetResultStatus<ContectusViewModel>(null, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<ContectusViewModel>(null, MessageStatus.NotExists, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<ContectusViewModel>(null, MessageStatus.SomethingWentWrong, false);
            }

            return objReturn;
        }

        //public async Task<ServiceResponse<AccountDeactivationViewModel>> DeActivateAccountRequest(AccountDeactivationViewModel model, string baseUrl = "")
        //{
        //    ServiceResponse<AccountDeactivationViewModel> objReturn = new ServiceResponse<AccountDeactivationViewModel>();
        //    try
        //    {
        //        var user = (await accountService.GetProfileDetails(model.UserId.ToString()));
        //        if (user is null)
        //        {
        //            objReturn = this.SetResultStatus<AccountDeactivationViewModel>(null, MessageStatus.NotExists, false);
        //        }

        //        string countryCode = string.Empty;
        //        try
        //        {
        //            var country = _unitOfWork.Instance.tblCountry.Where(x => x.Id == user.Data.MobileCountryCode).FirstOrDefault();
        //            countryCode = country?.DialCode;
        //        }
        //        catch
        //        {
        //            // HANDLING COUNTREY EXCEPTION
        //        }

        //        var enquiry = new Enquiry()
        //        {
        //            CreatedOn = DateTime.UtcNow,
        //            Message = model.Description,
        //            FullName = $"{user.Data.FirstName} {user.Data.LastName}",
        //            PhoneNo = (string.IsNullOrEmpty(user.Data.PhoneNumber) ? null : $"{countryCode} {user.Data.PhoneNumber}"),
        //            Email = user.Data.Email,
        //            Status = EnumEnquiryStatus.Open,
        //            EnquiryType = EnumEnquiryType.DeactivationRequest,
        //            UserId = user.Data.Id,

        //        };
        //        _unitOfWork.GenericRepository<Enquiry>().Add(enquiry);

        //        if (this._unitOfWork.SaveChanges())
        //        {
        //            await _emailService.DeactivationNotification(enquiry.Email, baseUrl); 
        //            await _emailService.EnquiryNotificationNew(new EnquiryNotificationRequest()
        //            {
        //                Date = enquiry.CreatedOn.GetDateTimeStringWithTime(),
        //                Email = enquiry.Email,
        //                UserName = enquiry.FullName,
        //                EnquiryID = enquiry.EnquiryCode.ToString(),
        //                RequestType = enquiry.EnquiryType,
        //                status = enquiry.Status
        //            });
        //        }

        //        objReturn = this.SetResultStatus<AccountDeactivationViewModel>(model, MessageStatus.Success, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        _exceptionLoggerService.LogException(ex);
        //        objReturn = this.SetResultStatus<AccountDeactivationViewModel>(null, MessageStatus.SomethingWentWrong, false);
        //    }
        //    return objReturn;
        //}


        //public async Task<ServiceResponse<List<EnquiryResponseViewModel>>> GetEnquiryResponseList(long enquiryID)
        //{
        //    ServiceResponse<List<EnquiryResponseViewModel>> objReturn = new ServiceResponse<List<EnquiryResponseViewModel>>();
        //    try
        //    {
        //        var enquiryResponse = (from enq in this._unitOfWork.Instance.EnquiryResponse
        //                               join user in this._unitOfWork.Instance.Users on enq.CreatedBy equals user.Id
        //                               where enq.EnquiryID == enquiryID
        //                               orderby enq.CreatedOn descending
        //                               select new EnquiryResponseViewModel()
        //                               {
        //                                   EnquiryID = enq.EnquiryID,
        //                                   Answer = enq.Answer,
        //                                   SendMail = enq.SendMail,
        //                                   StatusStr = EnumHelper.GetEnumDescription(enq.Status),
        //                                   Status = enq.Status,
        //                                   CreatedByUserName = user.UserName,
        //                                   CreatedOn = enq.CreatedOn
        //                               }).ToList();

        //        if (enquiryResponse.Count > 0)
        //        {
        //            objReturn = this.SetResultStatus<List<EnquiryResponseViewModel>>(enquiryResponse, MessageStatus.Success, true);

        //        }
        //        else
        //        {
        //            objReturn = this.SetResultStatus<List<EnquiryResponseViewModel>>(null, MessageStatus.NotExists, false);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _exceptionLoggerService.LogException(ex);
        //        objReturn = this.SetResultStatus<List<EnquiryResponseViewModel>>(null, MessageStatus.SomethingWentWrong, false);
        //    }
        //    return objReturn;
        //}


        public async Task<ServiceResponse<string>> CheckAccountDeactivationByUserId(Guid userId)
        {
            try
            {
                var IsAccountDeactivationRequested = _unitOfWork.Instance.Enquiry.
                                                Any(x => x.UserId == userId && !(x.Status == EnumEnquiryStatus.Rejected || x.Status == EnumEnquiryStatus.Deactivated) && x.EnquiryType == EnumEnquiryType.DeactivationRequest);

                return this.SetResultStatus<string>(null, MessageStatus.Success, IsAccountDeactivationRequested);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                return this.SetResultStatus<string>(null, MessageStatus.SomethingWentWrong, false);
            } 
        }
       
        public async Task<ServiceResponse<List<FaqViewModel>>> GetFaq()
        {
            ServiceResponse<List<FaqViewModel>> objReturn = new ServiceResponse<List<FaqViewModel>>();
            try
            {

                var dd = _unitOfWork.Instance.FAQ.ToList();
                var contentResponse = _unitOfWork.Instance.FAQ.OrderBy(o=>o.Order).Select(content => new FaqViewModel()
                {
                    Id = content.Id,
                    Question = content.Question,
                    Answer = content.Answer,
                    Order = content.Order
                }).ToList();

                if (contentResponse.Count > 0)
                {
                    objReturn = this.SetResultStatus<List<FaqViewModel>>(contentResponse, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<List<FaqViewModel>>(new List<FaqViewModel>(), MessageStatus.NotExists, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<List<FaqViewModel>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }
    }
}

