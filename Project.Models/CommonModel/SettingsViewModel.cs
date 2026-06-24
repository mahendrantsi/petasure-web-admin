using Project.Core.Enum;
using Project.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class SettingsViewModel
    {
        public SettingsViewModel()
        {

        }

        public SettingsViewModel(Data.DBEntities.Settings setting)
        {
            IsEmailConfirmed = setting.IsEmailConfirmed;
            IsPhoneVerificationRequired = setting.IsPhoneVerificationRequired;
            Id = setting.Id;
        }

        public Guid Id { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneVerificationRequired { get; set; }
 

        public List<SettingSelectedRoleViewModel> settingSelectedRoleViewModels { get; set; } = new List<SettingSelectedRoleViewModel>();
        public List<DocumentSettingViewModel> documentSettingViewModels { get; set; } = new List<DocumentSettingViewModel>();


    }

    public class SettingSelectedRoleViewModel
    {
        public long Id { get; set; }
        public string Role { get; set; }
        public bool selected { get; set; }

    }

    public class DocumentSettingViewModel
    {
        public long? Id { get; set; }
        public long DocTypeId { get; set; }
        public string DocumentForString { get; set; }
        [Required]
        public string DocType { get; set; }
        public bool IsRequire { get; set; }
        [Required]
        [RegularExpression(Setting.CheckValidString, ErrorMessage = Setting.CheckValidStringMsg)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }
        public string CreatedOnStr { get; set; }
        public string CreatedByStr { get; set; }

    }


   
}
