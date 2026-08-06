using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Models.AdminModel;
using Project.Persistence.UOW;
using Project.Services.ServiceEntities;
using Project.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services.Service
{
    public class AlertCentreService : BaseService, IAlertCentreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _imageBaseUrl;

        public AlertCentreService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            // Illness images are saved/served by Project.WebAPI, a separate host from this
            // admin dashboard app — see UserService's identical ResolveImageUrl for why a
            // bare relative path 404s here, and PetRepository.cs for the established
            // baseURL-prefix convention this mirrors.
            _imageBaseUrl = (configuration["CustomKeys:BaseUrl"] ?? string.Empty).TrimEnd('/');
        }

        private string ResolveImageUrl(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return relativePath;
            if (relativePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                relativePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return relativePath;
            }
            return _imageBaseUrl + "/" + relativePath.TrimStart('/');
        }

        // There is no dedicated "Alerts" table yet — an alert is derived from any illness
        // scan whose highest-confidence finding has Severity >= 2 (Medium/High per
        // HealthStatus's 1..3 scale). This is a pragmatic stand-in until a real
        // admin-review/alert-lifecycle workflow (dismiss, snooze, etc.) is added.
        private List<AlertItemViewModel> GetAlertsFromHealthChecks()
        {
            return _unitOfWork.Instance.HealthCheckEvents
                .Include(e => e.Pet)
                .Include(e => e.HealthStatuses)
                .Where(e => e.HealthStatuses.Any(h => h.Severity >= 2))
                .OrderByDescending(e => e.CreatedOn)
                .AsEnumerable()
                .Select(e =>
                {
                    var topFinding = e.HealthStatuses.OrderByDescending(h => h.Severity).First();
                    return new AlertItemViewModel
                    {
                        AlertId = e.Id,
                        PetId = e.PetId,
                        PetName = e.Pet != null ? e.Pet.PName : "Unknown",
                        PetType = e.Species.ToString(),
                        PetImageUrl = string.IsNullOrEmpty(e.ImageRef) ? "~/images/pet-placeholder.svg" : ResolveImageUrl(e.ImageRef),
                        ObservableSymptoms = topFinding.ConditionName,
                        Status = topFinding.Severity >= 3 ? "Vet Appointment Recommended" : "Alert Sent",
                        IsNew = e.Status == Project.Core.Enum.EnumHealthCheckStatus.Pending,
                        AlertTime = e.CreatedOn,
                    };
                })
                .ToList();
        }

        public Task<ServiceResponse<AlertCentreViewModel>> GetAlerts(AlertFilterViewModel filter = null)
        {
            try
            {
                var allAlerts = GetAlertsFromHealthChecks();
                var filteredAlerts = allAlerts;
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                // Apply filters if provided
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Status) && filter.Status != "All Status")
                    {
                        filteredAlerts = filteredAlerts.Where(a => a.Status == filter.Status).ToList();
                    }
                }

                var pageNumber = filter?.PageNumber ?? 1;
                var pageSize = 10;
                var totalRecords = filteredAlerts.Count;

                // Apply pagination
                var pagedAlerts = filteredAlerts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Calculate statistics
                var stats = new AlertStatisticsViewModel
                {
                    TotalAlerts = allAlerts.Count,
                    NewAlerts = allAlerts.Count(a => a.IsNew),
                    AlertSent = allAlerts.Count(a => a.Status == "Alert Sent"),
                    VetAppointmentRecommendedAlerts = allAlerts.Count(a => a.Status == "Vet Appointment Recommended")
                };

                var viewModel = new AlertCentreViewModel
                {
                    Alerts = filteredAlerts,
                    Statistics = stats,
                    TotalRecords = totalRecords,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };

                return Task.FromResult(new ServiceResponse<AlertCentreViewModel>
                {
                    IsSuccess = true,
                    Message = "Alerts retrieved successfully",
                    Data = viewModel
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ServiceResponse<AlertCentreViewModel>
                {
                    IsSuccess = false,
                    Message = $"Error retrieving alerts: {ex.Message}",
                    Data = null
                });
            }
        }

        public Task<ServiceResponse<AlertCentreViewModel>> GetAlertsByPage(int pageNumber, int pageSize, string status = null)
        {
            try
            {
                var allAlerts = GetAlertsFromHealthChecks();
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);
                var stats = new AlertStatisticsViewModel
                {
                    TotalAlerts = allAlerts.Count,
                    NewAlerts = allAlerts.Count(a => a.IsNew),
                    AlertSent = allAlerts.Count(a => a.Status == "Alert Sent"),
                    VetAppointmentRecommendedAlerts = allAlerts.Count(a => a.Status == "Vet Appointment Recommended")
                };

                // Apply status filter before paging
                var filteredAlerts = allAlerts;
                var activeStatus = "All Status";
                if (!string.IsNullOrEmpty(status) && status != "All Status")
                {
                    filteredAlerts = filteredAlerts.Where(a => a.Status == status).ToList();
                    activeStatus = status;
                }

                var pagedAlerts = filteredAlerts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new AlertCentreViewModel
                {
                    Alerts = pagedAlerts,
                    Statistics = stats,
                    TotalRecords = filteredAlerts.Count,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    ActiveStatus = activeStatus
                };

                return Task.FromResult(new ServiceResponse<AlertCentreViewModel>
                {
                    IsSuccess = true,
                    Message = "Alerts retrieved successfully",
                    Data = viewModel
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ServiceResponse<AlertCentreViewModel>
                {
                    IsSuccess = false,
                    Message = $"Error retrieving alerts: {ex.Message}",
                    Data = null
                });
            }
        }
        public async Task<ServiceResponse<AlertDetailViewModel>> GetAlertDetail(Guid alertId)
        {
            try
            {
                var healthEvent = await _unitOfWork.Instance.HealthCheckEvents
                    .Include(e => e.Pet)
                    .Include(e => e.HealthStatuses)
                    .FirstOrDefaultAsync(e => e.Id == alertId);

                if (healthEvent == null)
                {
                    return new ServiceResponse<AlertDetailViewModel>
                    {
                        IsSuccess = false,
                        Message = "Alert not found",
                        Data = null
                    };
                }

                if (healthEvent.Status == Project.Core.Enum.EnumHealthCheckStatus.Pending)
                {
                    healthEvent.Status = Project.Core.Enum.EnumHealthCheckStatus.Reviewed;
                    await _unitOfWork.SaveChangesAsync();
                }

                var topFinding = healthEvent.HealthStatuses
                    .OrderByDescending(h => h.Severity)
                    .ThenByDescending(h => h.Confidence)
                    .FirstOrDefault();

                var detail = new AlertDetailViewModel
                {
                    AlertId = healthEvent.Id,
                    PetName = healthEvent.Pet != null ? healthEvent.Pet.PName : "Unknown",
                    PetType = healthEvent.Species.ToString(),
                    ObservableSymptoms = topFinding?.ConditionName ?? "N/A",
                    AiConfidenceScore = topFinding?.Confidence ?? 0,
                    Status = topFinding != null && topFinding.Severity >= 3
                        ? "Vet Appointment Recommended"
                        : "Alert Sent",
                    AlertTime = healthEvent.CreatedOn,
                    CurrentImageUrl = string.IsNullOrEmpty(healthEvent.ImageRef)
                        ? null
                        : ResolveImageUrl(healthEvent.ImageRef),
                    PreviousImageUrl = string.IsNullOrEmpty(healthEvent.PreviousImageRef)
                        ? null
                        : ResolveImageUrl(healthEvent.PreviousImageRef),
                    Severity = topFinding?.Severity ?? 0,
                    AffectedArea = topFinding?.AffectedArea,
                    AiSummary = healthEvent.AiSummary
                };

                return new ServiceResponse<AlertDetailViewModel>
                {
                    IsSuccess = true,
                    Message = "Alert detail retrieved successfully",
                    Data = detail
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AlertDetailViewModel>
                {
                    IsSuccess = false,
                    Message = $"Error retrieving alert detail: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
