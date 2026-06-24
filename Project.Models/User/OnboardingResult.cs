using Project.Core.Enum;
using Project.Data.ExtendedDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Project.Models.User
{
    public class OnboardingResult
    {
      

        public OnboardingResult(Project.Models.AdminRule.AdminRule _rule, DerivedIdentityUser _user)
        {
            user = _user;
            if (_rule is not null)
            {

                IsOnboardCheck = true; 
                IsOnboard = _rule.UserOnboarding; 
                EmailRuleResult = _rule.EmailRuleResult;
            }
        }

        [JsonIgnore]
        public DerivedIdentityUser user { get;set; }
        public bool IsOnboardCheck { get; set; } = false;
        public bool IsProfileComplete { get; set; } = false;
        public bool IsOnboard { get; set; } = false;
        public bool KycRuleResult { get; set; }
        public bool KYBRuleResult { get; set; }
        public bool EmailRuleResult { get; set; }
    }


    public class LoginResult
    {
        public LoginResult(DerivedIdentityUser user)
        {
            User = user; 
            Email = user.Email;
            UserID = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ParentUserId = user.ParentUserID != null ? user.ParentUserID : null;
            UserImage = user.ImagePath;
            UserType = user.UserType.ToString();
        }
        [JsonIgnore]
        public DerivedIdentityUser User { get;private set; }
        public bool RequireTwoFactor{ get; set; }
        public string Email{ get; private set; }
        public Guid? ParentUserId { get; private set; }
        public Guid UserID{ get; private set; }
        public string FirstName{ get; private set; }
        public string LastName{ get; private set; }
        public JwtAuthResult Tokens { get; set; }
        public string MFAKey{ get; set; }
        public string UserImage{ get; set; }
        public string UserType { get; set; }
    }

    public class JwtAuthResult
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public RefreshToken RefreshToken { get; set; }

        public string UserName { get; set; }
    }

    public class RefreshToken
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }    //  can be used for usage tracking
        //  can optionally include other metadata, such as user agent, ip address, device name, and so on

        [JsonPropertyName("tokenString")]
        public string TokenString { get; set; }

        [JsonPropertyName("expireAt")]
        public DateTime ExpireAt { get; set; }
    }
}
