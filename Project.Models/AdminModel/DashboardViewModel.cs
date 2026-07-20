using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        // Scan analytics
        public int TotalScans { get; set; }
        public int MatchedScans { get; set; }
        public int UnmatchedScans { get; set; }
        public int ErrorCount { get; set; }

        // Error breakdown by stage
        public List<ErrorBreakdownItem> ErrorBreakdownItems { get; set; } = new List<ErrorBreakdownItem>();

        // Pet Scan Logs (paginated)
        public List<PetScanLogViewModel> PetScanLogs { get; set; } = new List<PetScanLogViewModel>();
        public int TotalScanLogs { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalScanLogs / PageSize) : 0;

        // Ill-health Review Metrics
        public int FlaggedSubmissions { get; set; }
        public int UnderReview { get; set; }
        public int Reviewed { get; set; }
        public int Resolved { get; set; }

        // Ill-health Reviews
        public List<IllHealthReviewViewModel> IllHealthReviews { get; set; } = new List<IllHealthReviewViewModel>();
        public string UserProfile { get; set; }
        public string UserName { get; set; }
        public List<MonthlyUsers> LstMonthlyUsers { get; set; } = new List<MonthlyUsers>();
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
        public string ScanType { get; set; }
        public string Notes { get; set; }
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

    public class ErrorBreakdownItem
    {
        public string Label { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }
}
