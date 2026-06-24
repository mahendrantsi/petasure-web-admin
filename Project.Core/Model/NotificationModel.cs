using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Model
{
    public class NotificationModel
    {
        public string UserId { get; set; }
        public string Sub { get; set; }
        public string Message { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NotificationImageUrl { get; set; }
        public string Email { get; set; }
        public IFormFile NotificationImage { get; set; }
        public long Id { get; set; }
        public string TierId { get; set; }
        public string LocationId { get; set; }
        public string PrefLocationId { get; set; }
        public string NotificationCategory { get; set; }
        public string NotificationCategoryId { get; set; }
        public string NotificationExpiryDate { get; set; }
        public string NotificationDate { get; set; }
        public bool NotificationSentStatus { get; set; }
    }
}
