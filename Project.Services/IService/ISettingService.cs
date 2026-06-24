using Project.Models.CommonModel;
using Project.Models.Master;
using Project.Services.Service;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface ISettingService
    {
        //Task<ServiceResponse<SettingsViewModel>> GetSettings();

        //Task<ServiceResponse<SettingsViewModel>> UpdateSetting(SettingsViewModel model);
        //Task<ServiceResponse<List<DocumentTypeViewModel>>> GetAllDocumentType();
        //Task<ServiceResponse<SettingsViewModel>> GetSettingsForAdmin();
        Task<ServiceResponse<SettingsViewModel>> GetSettings();
        Task<ServiceResponse<List<FAQViewModel>>> GetFAQ(bool GetOnlyActive = false);
        Task<ServiceResponse<FAQViewModel>> InsertFAQ(FAQViewModel model);
        Task<ServiceResponse<FAQViewModel>> UpdateFAQ(FAQViewModel model);
        Task<ServiceResponse<FAQViewModel>> GetFAQbyID(Guid id);

        Task<ServiceResponse<FAQViewModel>> DeleteFAQ(Guid id);
        Task<ServiceResponse<string>> UpdateAllFaqOrder(List<FaqOrder> request);
        Task<ServiceResponse<List<CountryCodesViewModel>>> GetActiveCountries(string basePath);

    }
}
