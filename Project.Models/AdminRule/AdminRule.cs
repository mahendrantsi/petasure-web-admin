using Project.Core.Enum;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.CommonModel;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceStack.FluentValidation.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Project.Core.Extension;

namespace Project.Models.AdminRule
{
    public class AdminRule
    {
        public AdminRule(DerivedIdentityUser user, SettingsViewModel AdminSetting,string role)
        {
            Role = role;
        SetUserProp:
            {
                UserEmailVerificationStatus = user.EmailConfirmed;
                UserPhoneVerificationStatus = user.PhoneNumberConfirmed;
            }
        SetSettingProp:
            {
                EmailVerificationReqForOnboarding = AdminSetting.IsEmailConfirmed;
                PhoneVerificationReqForOnboarding = AdminSetting.IsPhoneVerificationRequired;
            }
            this.SetOnboarding();
        }

        public List<string> Errors { get; set; } = new List<string>();

        private string Role { get; set; }
        public bool UserEmailVerificationStatus { get; set; }
        public bool UserPhoneVerificationStatus { get; set; }


        #region EMAIL VERIFICATION
        public bool EmailRuleResult { get; set; }
        private bool _EmailVerificationReqForOnboarding { get; set; }
        public bool EmailVerificationReqForOnboarding
        {
            get => _EmailVerificationReqForOnboarding;
            
            set
            {
                _EmailVerificationReqForOnboarding = value;
                this.EmailRuleResult = (!_EmailVerificationReqForOnboarding || (UserEmailVerificationStatus && _EmailVerificationReqForOnboarding));
                if (!this.EmailRuleResult) this.Errors.Add($"Email verification is required for onboarding.");
            }
        }
        #endregion

        #region PHONE VERIFICATION
        public bool PhoneRuleResult { get; set; }
        private bool _PhoneVerificationReqForOnboarding { get; set; }
        public bool PhoneVerificationReqForOnboarding
        {
            get=>_PhoneVerificationReqForOnboarding;
            set
            {
                _PhoneVerificationReqForOnboarding = value;
                this.PhoneRuleResult = (!_PhoneVerificationReqForOnboarding || (UserPhoneVerificationStatus && _PhoneVerificationReqForOnboarding));
                if(!this.PhoneRuleResult) this.Errors.Add($"Phone number verification is required for onboarding.");
            }
        }
        #endregion


        public bool UserOnboarding { get; private set; }
        private void SetOnboarding() => this.UserOnboarding = (Role == EnumRole.Admin.GetDescription()||(Role != EnumRole.Admin.GetDescription() && this.EmailRuleResult && this.PhoneRuleResult));

    }
}
