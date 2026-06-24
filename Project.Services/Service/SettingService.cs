using AutoMapper;
using Project.Core.Enum;
using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.CommonModel;
using Project.Models.Master;
using Project.Persistence.UOW;
using Project.Services.IService;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.Services.Service.SettingService;

namespace Project.Services.Service
{
    public class SettingService : BaseService, ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        public SettingService(IUnitOfWork unitOfWork, IMapper mapper, IExceptionLoggerService exceptionLoggerService)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
            _exceptionLoggerService = exceptionLoggerService;
        }
        public async Task<ServiceResponse<SettingsViewModel>> GetSettings()
        {
            ServiceResponse<SettingsViewModel> objReturn = new ServiceResponse<SettingsViewModel>();
            try
            {
                using (var db = new ProjectDbContext())
                {
                    var settingListResults = db.Settings.FirstOrDefault();

                    if (settingListResults != null)
                    {
                        var settingViewModels = _mapper.Map<SettingsViewModel>(settingListResults); 
                        List<string> userEnum = Enum.GetValues(typeof(EnumRole)).Cast<EnumRole>().Select(v => v.ToString()).Where(x =>   x == EnumRole.User.ToString()).ToList();
                        //foreach (EnumSettingSelectionType type in Enum.GetValues(typeof(EnumSettingSelectionType)))
                        //{
                        //    foreach (var role in userEnum)
                        //    {
                        //        settingViewModels.settingSelectedRoleViewModels.Add(new SettingSelectedRoleViewModel
                        //        {
                        //            Role = role.ToString(),
                        //            type = type,
                        //            selected = false
                        //        });
                        //    }
                        //}
                        objReturn = this.SetResultStatus<SettingsViewModel>(settingViewModels, MessageStatus.Success, true);
                    }
                    else
                        objReturn = this.SetResultStatus<SettingsViewModel>(null, MessageStatus.NotExists, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<SettingsViewModel>(null, MessageStatus.Error, false);
            }
            return objReturn;
        }
        public async Task<ServiceResponse<List<FAQViewModel>>> GetFAQ(bool GetOnlyActive = false)
        {
            ServiceResponse<List<FAQViewModel>> objReturn = new();
            try
            {
                var fAQ = (from faq in _unitOfWork.GenericRepository<FAQ>().Get()
                           join user in _unitOfWork.GenericRepository<DerivedIdentityUser>().Get() on faq.CreatedBy equals user.Id
                           orderby faq.CreatedOn descending
                           where !faq.IsDeleted && (!GetOnlyActive || GetOnlyActive && faq.IsActive)
                           select new FAQViewModel()
                           {
                               Id = faq.Id,
                               Question = faq.Question,
                               Answer = faq.Answer,
                               CreatedBy = faq.CreatedBy,
                               IsActive = faq.IsActive,
                               CreatedByStr = user.UserName,
                               CreatedOnStr = faq.CreatedOn.ToString("dd/MM/yyyy"),
                               Order = faq.Order
                           }).ToList();
                objReturn = this.SetResultStatus<List<FAQViewModel>>(fAQ, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<List<FAQViewModel>>(null, MessageStatus.Error, false);
            }
            return objReturn;
        }

        public async Task<ServiceResponse<FAQViewModel>> GetFAQbyID(Guid id)
        {
            ServiceResponse<FAQViewModel> objReturn = new();
            try
            {
                var fAQ = (from faq in _unitOfWork.GenericRepository<FAQ>().Get()
                           join user in _unitOfWork.GenericRepository<DerivedIdentityUser>().Get() on faq.CreatedBy equals user.Id
                           orderby faq.CreatedOn descending
                           where !faq.IsDeleted && faq.Id == id
                           select new FAQViewModel()
                           {
                               Id = faq.Id,
                               Question = faq.Question,
                               Answer = faq.Answer,
                               CreatedBy = faq.CreatedBy,
                               IsActive = faq.IsActive,
                               CreatedByStr = user.UserName,
                               CreatedOnStr = faq.CreatedOn.ToString("dd/MM/yyyy"),
                               Order = faq.Order
                           }).FirstOrDefault();
                objReturn = this.SetResultStatus<FAQViewModel>(fAQ, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<FAQViewModel>(null, MessageStatus.Error, false);
            }
            return objReturn;
        }

        public async Task<ServiceResponse<FAQViewModel>> InsertFAQ(FAQViewModel model)
        {
            try
            {
                var order = 1;

                var currentTopFAQ = _unitOfWork.GenericRepository<FAQ>().Find(x => !x.IsDeleted).OrderByDescending(x => x.Order).FirstOrDefault();
                if (currentTopFAQ is not null)
                {
                    order = currentTopFAQ.Order + 1;
                }
                model.Order = order;

                var fAQ = _mapper.Map<FAQ>(model);
                fAQ.CreatedOn = DateTime.UtcNow;
                _unitOfWork.GenericRepository<FAQ>().Add(fAQ);
                await _unitOfWork.SaveChangesAsync();
                model.Id = fAQ.Id;
                return this.SetResultStatus<FAQViewModel>(model, "FAQ created successfully.", true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                return this.SetResultStatus<FAQViewModel>(null, MessageStatus.Error, false);
            }
        }

        public async Task<ServiceResponse<FAQViewModel>> UpdateFAQ(FAQViewModel model)
        {
            try
            {
                var fAQ = _unitOfWork.GenericRepository<FAQ>().Find(x => x.Id == model.Id).FirstOrDefault();
                if (fAQ is null)
                {
                    return this.SetResultStatus<FAQViewModel>(null, "FAQ not fount.", false);
                }

                fAQ.Question = model.Question;
                fAQ.Answer = model.Answer;
                fAQ.IsActive = model.IsActive;
                _unitOfWork.GenericRepository<FAQ>().UpdateEntity(fAQ);
                await _unitOfWork.SaveChangesAsync();
                model.Id = fAQ.Id;
                return this.SetResultStatus<FAQViewModel>(model, "FAQ updated successfully.", true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                return this.SetResultStatus<FAQViewModel>(null, MessageStatus.Error, false);
            }
        }

        public async Task<ServiceResponse<FAQViewModel>> DeleteFAQ(Guid id)
        {
            try
            {
                var fAQ = _unitOfWork.GenericRepository<FAQ>().Find(x => x.Id == id).FirstOrDefault();
                if (fAQ is null)
                {
                    return this.SetResultStatus<FAQViewModel>(null, "FAQ not fount.", false);
                }
                fAQ.IsDeleted = true;
                _unitOfWork.GenericRepository<FAQ>().UpdateEntity(fAQ);
                await _unitOfWork.SaveChangesAsync();
                return this.SetResultStatus<FAQViewModel>(_mapper.Map<FAQViewModel>(fAQ), "FAQ deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                return this.SetResultStatus<FAQViewModel>(null, MessageStatus.Error, false);
            }
        }

        public async Task<ServiceResponse<string>> UpdateAllFaqOrder(List<FaqOrder> request)
        {
            try
            {
                var faqIds = request.Select(x => x.id).ToArray();

                var FAQS = _unitOfWork.GenericRepository<FAQ>().Get(x => faqIds.Contains(x.Id)).ToList();

                if (FAQS.Count > 0)
                {
                    FAQS.ForEach(x =>
                    {
                        var currentFAQ = request.FirstOrDefault(y => y.id == x.Id);
                        x.Order = currentFAQ.order;
                    });

                    _unitOfWork.GenericRepository<FAQ>().UpdateRange(FAQS);
                    await _unitOfWork.SaveChangesAsync();

                    return this.SetResultStatus<string>(null, MessageStatus.Success, true);
                }
                else
                {
                    return this.SetResultStatus<string>(null, MessageStatus.NotExists, false);
                }
            }
            catch (Exception ex)
            {
                return this.SetResultStatus<string>(null, MessageStatus.Error, false);
            }
        }

        public async Task<ServiceResponse<List<CountryCodesViewModel>>> GetActiveCountries(string basePath)
        {
            ServiceResponse<List<CountryCodesViewModel>> objReturn = new ServiceResponse<List<CountryCodesViewModel>>();
            try
            {
                var country = _unitOfWork.GenericRepository<tblCountry>()
                    .Get(x => x.IsActive)
                    .Select(x => new CountryCodesViewModel()
                    {
                        Id = x.Id,
                        ShortCode = x.ShortCode,
                        CountryName = x.CountryName,
                        ImageUrl = $"{basePath}{"images/Country-flags"}/{x.CountryName.Replace(' ', '_')}.png",
                        DialCode = x.DialCode
                    }).ToList();
                objReturn = this.SetResultStatus<List<CountryCodesViewModel>>(country, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<List<CountryCodesViewModel>>(null, MessageStatus.Error, false);
            }
            return objReturn;
        }

    }
}
public class FaqOrder
{
    public Guid id { get; set; }
    public int order { get; set; }

}
