using System;
using System.Collections.Generic;

namespace Project.Models.AdminModel
{
    public class AlertCentreViewModel
    {
        public List<AlertItemViewModel> Alerts { get; set; } = new List<AlertItemViewModel>();
        public AlertStatisticsViewModel Statistics { get; set; } = new AlertStatisticsViewModel();
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string ActiveStatus { get; set; } = "All Status";
    }

    public class AlertStatisticsViewModel
    {
        public int TotalAlerts { get; set; }
        public int NewAlerts { get; set; }
        public int AlertSent { get; set; }
        public int VetAppointmentRecommendedAlerts { get; set; }
    }

    public class AlertItemViewModel
    {
        public Guid AlertId { get; set; }
        public Guid? PetId { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; } // Dog, Cat
        public string PetImageUrl { get; set; }
        public string ObservableSymptoms { get; set; }
        public string Status { get; set; } // Alert Sent, Vet Appointment Recommended
        public DateTime AlertTime { get; set; }
        public bool IsNew { get; set; }
        public string FormattedAlertTime => AlertTime.ToString("hh:mm tt") + "\n" + AlertTime.ToString("MMM d, yyyy");
    }

    public class AlertFilterViewModel
    {
        public string Status { get; set; } = "All Status";
        public string Search { get; set; }
        public int PageNumber { get; set; } = 1;
    }
}
