using System;

namespace Project.Models.AdminModel
{
    public class AlertDetailViewModel
    {
        public Guid AlertId { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; } // Dog, Cat
        public string ObservableSymptoms { get; set; }
        public decimal AiConfidenceScore { get; set; } // 0–1 range
        public int AiConfidencePercent => (int)Math.Round(AiConfidenceScore * 100);
        public string Status { get; set; } // Alert Sent, Vet Appointment Recommended
        public DateTime AlertTime { get; set; }
        public string CurrentImageUrl { get; set; }
        public string PreviousImageUrl { get; set; }
        public int Severity { get; set; } // 1 (low) .. 3 (high)
        public string AffectedArea { get; set; }
        public string AiSummary { get; set; }
    }
}
