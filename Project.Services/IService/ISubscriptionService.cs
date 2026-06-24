using Project.Core.Enum;
using Project.Data.DBEntities;
using Project.Models.Pets;
using Project.Models.Subscription;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface ISubscriptionService
    {
        Task<ServiceResponse<ApiSubscriptionResponseModel>> GetActiveSubscriptions(Guid userID);
        Task<ServiceResponse<List<ApiSubscriptionResponseModel>>> GetActiveSubscriptionList(Guid userID, EnumUserType userType);


        Task<ServiceResponse<string>> SaveSubscription(SubscriptionViewModel data);
        Task<ServiceResponse<string>> CancelSubscription(SubscriptionViewModel model);
        Task<ServiceResponse<string>> ActivateSubscription(SubscriptionViewModel model);
        Task<ServiceResponse<string>> SkippedSubscription(SubscriptionViewModel model);
        Task<ServiceResponse<string>> UnSkippedSubscription(SubscriptionViewModel model);
        Task<bool> DeleteAllSubscriptions(int customerId);

        Task<ServiceResponse<string>> SaveInAppPurchase(InAppPurchaseInputViewModel data);

        Task<ServiceResponse<string>> IsCertificateValid(Guid userid, bool isSandBox);

    }
}
