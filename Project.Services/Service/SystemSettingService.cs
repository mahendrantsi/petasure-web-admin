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
using Project.Services.ServiceHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.Services.Service.SettingService;

namespace Project.Services.Service
{
    public class SystemSetting : BaseService, ISystemSetting
    { 

        public SystemSetting()
        { 
        }

        public bool IsEmailConfirmed { get;private set; }
        public bool IsPhoneVerificationRequired { get;private set; }
        public void SetSystemVariables(SettingsViewModel model)
        {
            IsEmailConfirmed = model.IsEmailConfirmed;
            IsPhoneVerificationRequired = model.IsPhoneVerificationRequired; 
        }

        public SettingsViewModel GetSystemVariables()=> new SettingsViewModel { IsEmailConfirmed = IsEmailConfirmed, IsPhoneVerificationRequired = IsPhoneVerificationRequired };


    }
} 