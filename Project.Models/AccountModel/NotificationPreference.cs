using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AccountModel
{
    public class NotificationPreferenceReqDTO
    {
        [Required]
        public bool AppNotifications { get; set; }
        [Required]
        public bool SMSNotifications { get; set; }
        [Required]
        public bool EMailNotifications { get; set; }
    }
}
