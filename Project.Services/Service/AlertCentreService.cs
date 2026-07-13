using Project.Models.AdminModel;
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
        private static List<AlertItemViewModel> GetDummyAlerts()
        {
            return new List<AlertItemViewModel>
            {
                new AlertItemViewModel
                {
                    AlertId = 1,
                    PetId = 101,
                    PetName = "Buddy",
                    PetType = "Dog",
                    PetImageUrl = "~/images/pets/buddy.jpg",
                    ObservableSymptoms = "Body Composition Change",
                    Status = "Alert Sent",
                    AlertTime = DateTime.Now.AddHours(-2)
                },
                new AlertItemViewModel
                {
                    AlertId = 2,
                    PetId = 102,
                    PetName = "Luna",
                    PetType = "Cat",
                    PetImageUrl = "~/images/pets/luna.jpg",
                    ObservableSymptoms = "Hair Loss",
                    Status = "Vet Appointment Recommended",
                    AlertTime = DateTime.Now.AddHours(-4)
                },
                new AlertItemViewModel
                {
                    AlertId = 3,
                    PetId = 103,
                    PetName = "Charlie",
                    PetType = "Dog",
                    PetImageUrl = "~/images/pets/charlie.jpg",
                    ObservableSymptoms = "Suspected Lump",
                    Status = "Alert Sent",
                    AlertTime = DateTime.Now.AddHours(-3)
                },
                new AlertItemViewModel
                {
                    AlertId = 4,
                    PetId = 104,
                    PetName = "Whiskers",
                    PetType = "Cat",
                    PetImageUrl = "~/images/pets/whiskers.jpg",
                    ObservableSymptoms = "Eye Redness",
                    Status = "Vet Appointment Recommended",
                    AlertTime = DateTime.Now.AddHours(-5)
                },
                new AlertItemViewModel
                {
                    AlertId = 5,
                    PetId = 105,
                    PetName = "Max",
                    PetType = "Dog",
                    PetImageUrl = "~/images/pets/max.jpg",
                    ObservableSymptoms = "Weight Loss",
                    Status = "Alert Sent",
                    AlertTime = DateTime.Now.AddHours(-6)
                },
                new AlertItemViewModel
                {
                    AlertId = 6,
                    PetId = 106,
                    PetName = "Milo",
                    PetType = "Cat",
                    PetImageUrl = "~/images/pets/milo.jpg",
                    ObservableSymptoms = "Body Composition Change",
                    Status = "Vet Appointment Recommended",
                    AlertTime = DateTime.Now.AddHours(-1)
                },
                new AlertItemViewModel
                {
                    AlertId = 7,
                    PetId = 107,
                    PetName = "Rocky",
                    PetType = "Dog",
                    PetImageUrl = "~/images/pets/rocky.jpg",
                    ObservableSymptoms = "Hair Loss",
                    Status = "Alert Sent",
                    AlertTime = DateTime.Now.AddHours(-8)
                },
                new AlertItemViewModel
                {
                    AlertId = 8,
                    PetId = 108,
                    PetName = "Bella",
                    PetType = "Dog",
                    PetImageUrl = "~/images/pets/bella.jpg",
                    ObservableSymptoms = "Eye Redness",
                    Status = "Vet Appointment Recommended",
                    AlertTime = DateTime.Now.AddHours(-10)
                },
                new AlertItemViewModel
                {
                    AlertId = 9,
                    PetId = 109,
                    PetName = "Daisy",
                    PetType = "Cat",
                    PetImageUrl = "~/images/pets/daisy.jpg",
                    ObservableSymptoms = "Weight Loss",
                    Status = "Alert Sent",
                    AlertTime = DateTime.Now.AddHours(-7)
                },
                new AlertItemViewModel
                {
                    AlertId = 10,
                    PetId = 110,
                    PetName = "Oliver",
                    PetType = "Cat",
                    PetImageUrl = "~/images/pets/oliver.jpg",
                    ObservableSymptoms = "Suspected Lump",
                    Status = "Vet Appointment Recommended",
                    AlertTime = DateTime.Now.AddHours(-9)
                }
            };
        }

        public Task<ServiceResponse<AlertCentreViewModel>> GetAlerts(AlertFilterViewModel filter = null)
        {
            try
            {
                var allAlerts = GetDummyAlerts();
                var filteredAlerts = allAlerts;

                // Apply filters if provided
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Status) && filter.Status != "All Status")
                    {
                        filteredAlerts = filteredAlerts.Where(a => a.Status == filter.Status).ToList();
                    }
                }

                // Calculate statistics
                var stats = new AlertStatisticsViewModel
                {
                    TotalAlerts = allAlerts.Count,
                    NewAlerts = allAlerts.Count(a => a.Status == "Alert Sent"),
                    AlertSent = allAlerts.Count(a => a.Status == "Alert Sent"),
                    VetAppointmentRecommendedAlerts = allAlerts.Count(a => a.Status == "Vet Appointment Recommended")
                };

                var viewModel = new AlertCentreViewModel
                {
                    Alerts = filteredAlerts,
                    Statistics = stats,
                    TotalRecords = filteredAlerts.Count,
                    CurrentPage = filter?.PageNumber ?? 1,
                    PageSize = 10
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

        public Task<ServiceResponse<AlertCentreViewModel>> GetAlertsByPage(int pageNumber, int pageSize)
        {
            try
            {
                var allAlerts = GetDummyAlerts();

                var stats = new AlertStatisticsViewModel
                {
                    TotalAlerts = allAlerts.Count,
                    NewAlerts = allAlerts.Count(a => a.Status == "Alert Sent"),
                    AlertSent = allAlerts.Count(a => a.Status == "Alert Sent"),
                    VetAppointmentRecommendedAlerts = allAlerts.Count(a => a.Status == "Vet Appointment Recommended")
                };

                var pagedAlerts = allAlerts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new AlertCentreViewModel
                {
                    Alerts = pagedAlerts,
                    Statistics = stats,
                    TotalRecords = allAlerts.Count,
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
    }
}
