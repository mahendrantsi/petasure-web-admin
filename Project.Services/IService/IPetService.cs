using Project.Data.DBEntities;
using Project.Models.AccountModel;
using Project.Models.CommonModel;
using Project.Models.GeneralModel;
using Project.Models.Pets;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IPetService
    {
        /// <summary>
        /// Get Pet Listing Service
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<ServiceResponse<List<PetsViewModel>>> petInfos(Guid userID);
        Task<ServiceResponse<List<PetsViewModel>>> AdminPetInfos();


        /// <summary>
        /// Get Pet Detail by Pet Id GUID
        /// </summary>
        /// <param name="petId"></param>
        /// <returns></returns>
        Task<ServiceResponse<PetsViewModel>> petDetail(Guid petId);


        /// <summary>
        /// Get Pet Detail by Pet Id String
        /// </summary>
        /// <param name="petId"></param>
        /// <returns></returns>
        Task<ServiceResponse<PetsViewModel>> petDetail(string petId, string baseURL);

        /// <summary>
        /// Get Pet Detail By microCip Number
        /// </summary>
        /// <param name="petId"></param>
        /// <returns></returns>
        Task<ServiceResponse<PetsViewModel>> petDetailByMicroNumber(String petId);

        /// <summary>
        /// Create New Pet Service
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        Task<ServiceResponse<PetsViewModel>> CreateNewPet(PetsViewModel petData);


        /// <summary>
        /// Create New Pet Service
        /// </summary>
        /// <param name="petId"></param>
        /// <returns></returns>
        Task<ServiceResponse<string>> DeletePet(Guid petId);

        /// <summary>
        /// Update Pet Info
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        Task<ServiceResponse<PetsViewModel>> UpdatePet(PetsViewModel petData);
        bool DeleteAllPets(Guid userId, string productTitle);

        Task<string> SimilarDogRequest(SimilarDogRequestViewModel model);
        Task<string> AnalyzeDogRequest(AnalyzeDogRequestViewModel model);
        Task<string> RegisterDogRequest(RegisterDogRequestViewModel model);
        Task<string> DeletePetsOnAI(List<Guid> petIds);

        Task<string> SimilarCatRequest(SimilarCatRequestViewModel model);
        Task<string> AnalyzeCatRequest(AnalyzeCatRequestViewModel model);
        Task<string> RegisterCatRequest(RegisterCatRequestViewModel model);
    }

   
}
