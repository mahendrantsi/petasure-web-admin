using Project.Core.Enum;
using Project.Models.CommonModel;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface INotificationService
    {
        Task<ServiceResponse<string>> Notify(long toUserId, string title, string body, long? transactionId, string baseUrl, EnumNotificationCategory category = EnumNotificationCategory.Transaction, string transactionCode = "");
        Task<ServiceResponse<List<NotificationViewModel>>> GetUserNotification(long userID, JQueryDataTableModel param);
        Task<ServiceResponse<Data.DBEntities.Notifications>> SaveNotification(long toUserId, string title, string body, long? transactionId, EnumNotificationCategory category = EnumNotificationCategory.Transaction);
        Task<ServiceResponse<string>> ReadNotification(long notificationID);
        Task<ServiceResponse<NotificationViewModel>> GetUserNotificationByID(long Id);
        Task<ServiceResponse<NotificationCountViewModel>> GetUserNotificationCount(long userID);
        Task<bool> SendSMS(string MobileWithDialCode, string Message);

    }
}
