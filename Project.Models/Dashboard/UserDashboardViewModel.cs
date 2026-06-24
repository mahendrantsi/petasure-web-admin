using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Dashboard
{

    public class UserDashboardViewModel
    {
        public string SpendAmount { get; set; } = "0.00";
        public string ReceiveAmount { get; set; } = "0.00";
        public string ReferralCode { get; set; }


        public string ReferralRuleTitle{ get; set; }    
        public string ReferralRuleSubTitle{ get; set; }    
        public UserRewardViewModel UserReward { get; set; }


        public List<UserDataViewModel> Favorites { get; set; }
        public List<UserDataViewModel> Recent { get; set; }
        public List<DBCommonDataViewModel> Business { get; set; }
        public List<DBCommonDataViewModel> Splits { get; set; }
        //public List<LoyaltyViewModel> Loyalty { get; set; }
    }

    public class UserRewardViewModel
    {  
        public string Cashback { get; set; }
        public string LoyaltyPoints { get; set; }
    }

    public class UserDataViewModel : DBCommonDataViewModel
    {

        public bool IsFavorites { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class DBCommonDataViewModel
    {
        public Guid? UserGuid { get; set; }
        public long Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
    }

    public class LoyaltyViewModel
    {
        public long Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string points { get; set; }
    }
}
