using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AdminRule
{
    public class ReferralRule
    {
        public long FormUser { get; set; }
        public long ToUser { get; set; }
        private readonly ProjectDbContext  dbContext;

        public ReferralRule(long formUser, long toUser)
        {

            FormUser = formUser;
            ToUser = toUser;
        }



        private void GetReferralRule() 
        {
            //var currentDate = DateTime.UtcNow;
            //var rule = dbContext.RewardRules.Where(x => x.Type == Core.Enum.EnumRewardType.Referral &&
            //                                            x.FromDate <= currentDate && currentDate <= x.ToDate &&
            //                                            x.IsActive).FirstOrDefault();

            //if (rule is not null)
            //{ 
            
            //}
        }
    }
}
