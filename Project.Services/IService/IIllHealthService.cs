using Project.Models.GeneralModel;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IIllHealthService
    {
        Task<ServiceResponse<IllHealthResponse>> AnalyzeAsync(IllHealthAnalyzeRequest request);

        Task<ServiceResponse<List<IllHealthHistoryEntry>>> GetHistoryAsync(string petId, Guid currentUserId);

        Task<string> TestPythonConnectionAsync();
    }
}
