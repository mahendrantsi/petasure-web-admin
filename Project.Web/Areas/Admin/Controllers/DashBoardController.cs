using Microsoft.AspNetCore.Mvc;
using Project.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Persistence.UOW;
using Project.Services.IService;

namespace Project.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [CustomAuthorize(Roles = "Admin,SubAdmin,AnonymousUser")]
    public class DashBoardController : BaseController
    {
        private readonly IUserService _userService;
        public DashBoardController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            @ViewData["Title"] = "Dashboard";
            var serviceResponse = await this._userService.GetAdminDashboard(page);
            return View(serviceResponse.Data);
        }

        public async Task<IActionResult> ScanLogs(int page = 1, string search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            @ViewData["Title"] = "Scan Logs";
            var serviceResponse = await this._userService.GetScanLogsAsync(page, search, fromDate, toDate);
            return View(serviceResponse.Data);
        }

        [HttpGet("Admin/DashBoard/DownloadScanLogsCsv")]
        public async Task<IActionResult> DownloadScanLogsCsv(string search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var response = await _userService.GetAllScanLogsAsync(search, fromDate, toDate);
            var logs = response.Data ?? new List<Project.Models.AdminModel.PetScanLogViewModel>();

            var csvBuilder = new System.Text.StringBuilder();
            csvBuilder.AppendLine("Pet Name,Pet Type,Result,Confidence,Scan Date,Scan Type,Notes");

            foreach (var log in logs)
            {
                csvBuilder.AppendLine(
                    $"{Escape(log.PetName)}," +
                    $"{Escape(log.PetType)}," +
                    $"{Escape(log.Result)}," +
                    $"{log.Confidence:F2}%," +
                    $"{log.ScanDate:yyyy-MM-dd HH:mm:ss}," +
                    $"{Escape(log.ScanType)}," +
                    $"{Escape(log.Notes)}"
                );
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return File(buffer, "text/csv", "ScanLogs.csv");
        }

        public async Task<IActionResult> IllHealthSubmissions(int page = 1, string search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            @ViewData["Title"] = "Ill-Health Submissions";
            var serviceResponse = await this._userService.GetIllHealthLogsAsync(page, search, fromDate, toDate);
            return View(serviceResponse.Data);
        }

        [HttpGet("Admin/DashBoard/DownloadIllHealthCsv")]
        public async Task<IActionResult> DownloadIllHealthCsv(string search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var response = await _userService.GetAllIllHealthLogsAsync(search, fromDate, toDate);
            var logs = response.Data ?? new List<Project.Models.AdminModel.IllHealthReviewViewModel>();

            var csvBuilder = new System.Text.StringBuilder();
            csvBuilder.AppendLine("Pet Name,Pet Type,Suggested Condition,Confidence,Status,AI Verdict,Submission Date");

            foreach (var log in logs)
            {
                csvBuilder.AppendLine(
                    $"{Escape(log.PetName)}," +
                    $"{Escape(log.PetType)}," +
                    $"{Escape(log.AISuggestedCondition)}," +
                    $"{log.Confidence:F2}%," +
                    $"{Escape(log.Status)}," +
                    $"{Escape(log.AIVerdict)}," +
                    $"{log.SubmissionDate:yyyy-MM-dd HH:mm:ss}"
                );
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return File(buffer, "text/csv", "IllHealthLogs.csv");
        }

        private static string Escape(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return $"\"{input.Replace("\"", "\"\"")}\"";
        }
    }
}
