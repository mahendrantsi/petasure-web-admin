using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AdminModel
{
    public class DashboardViewModel
    {

        public int TotalUsers { get; set; }
        public int MonthlyNewUsers { get; set; }
        public int NumberOfCats { get; set; }
        public int NumberOfDogs { get; set; }

        // Recognition Metrics
        public int RecognitionAttempts { get; set; }
        public decimal MatchRate { get; set; }
        public int TopUnmatchedScans { get; set; }
        public int ErrorBreakdown { get; set; }

        // Pet Scan Logs
        public List<PetScanLogViewModel> PetScanLogs { get; set; }

        // Ill-health Review Metrics
        public int FlaggedSubmissions { get; set; }
        public int UnderReview { get; set; }
        public int Reviewed { get; set; }
        public int Resolved { get; set; }

        // Ill-health Reviews
        public List<IllHealthReviewViewModel> IllHealthReviews { get; set; }

        public string UserProfile{ get; set; }
        public string UserName{ get; set; }
        public List<MonthlyUsers> LstMonthlyUsers { get; set; }
    }

    public class PetScanLogViewModel
    {
        public Guid Id { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; }
        public string PetImagePath { get; set; }
        public string Result { get; set; }
        public decimal Confidence { get; set; }
        public DateTime ScanDate { get; set; }
    }

    public class IllHealthReviewViewModel
    {
        public Guid Id { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; }
        public string PetImagePath { get; set; }
        public string AISuggestedCondition { get; set; }
        public decimal Confidence { get; set; }
        public string Status { get; set; }
        public string AIVerdict { get; set; }
        public string AdminOverride { get; set; }
        public string OverrideNotes { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

    public class MonthlyUsers
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal UserCount { get; set; }

    }
}
