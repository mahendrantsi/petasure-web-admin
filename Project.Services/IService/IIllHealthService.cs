using Project.Models.GeneralModel;
using Project.Services.ServiceEntities;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IIllHealthService
    {
        Task<ServiceResponse<IllHealthResponse>> AnalyzeAsync(IllHealthAnalyzeRequest request);
    }
}
