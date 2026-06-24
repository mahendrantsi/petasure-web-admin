using Project.Models.Pets;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IMissingService
    {
        Task<ServiceResponse<string>> ReportMissingPet(MissingPetRequestViewModel messingDetails, string userEmail);


        /// <summary>
        /// Found Missing Pet API
        /// Working for 2 things
        /// 1. For Owner (To change the status FOUND)
        /// 2. For Guest User who enter the entry for found
        /// 
        /// For condition 1 we have to send only pet Id and status
        /// For Condition 2 we have to send complete object
        /// </summary>
        /// <param name="messingDetails"></param>
        /// <returns></returns>
        Task<ServiceResponse<string>> FoundMyPet(FoundMissingPetRequest messingDetails);
        Task<ServiceResponse<string>> FoundMissingPet(FoundMissingPetRequest missingDetails);
        Task<ServiceResponse<string>> FoundMissingPetByAnonymous(FoundMissingPetRequest missingDetails);

        Task<ServiceResponse<List<MissingPetsViewModel>>> AdminMissingPetInfos();
        Task<ServiceResponse<List<IDCheckViewModel>>> AdminIDCheckPets();

    }
}
