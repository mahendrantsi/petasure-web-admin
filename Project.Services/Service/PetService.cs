using Project.Data.DBEntities;
using Project.Services.IService;
using Project.Services.Resources;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models.Pets;
using Project.Core.Extension;
using Project.Models.Master;
using Project.Core.Enum;
using Project.Data.ExtendedDBEntities;
using Project.Models.AccountModel;
using Project.Persistence.UOW;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using Project.Models.GeneralModel;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using ServiceStack;
using static System.Net.Mime.MediaTypeNames;


namespace Project.Services.Service
{

    public class PetService : BaseService, IPetService
    {
        //UnitOfWork Accessing for DB Tables
        private readonly IUnitOfWork _unitOfWork;

        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IConfiguration configuataion;

        public PetService(IUnitOfWork unitOfWork, IExceptionLoggerService exceptionLoggerService)
        {
            _unitOfWork = unitOfWork;
            _exceptionLoggerService = exceptionLoggerService;
            configuataion = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
        }


        /// <summary>
        /// Get Pet Listing according to currentUser Buisness Logic
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<List<PetsViewModel>>> petInfos(Guid userID)
        {
            var response = new ServiceResponse<List<PetsViewModel>>();
            try
            {
                var petInfoList = _unitOfWork.PetRepository.GetPetList(userID).OrderByDescending(a => a.CreatedOn).ToList();

                if (petInfoList != null)
                {
                    response = this.SetResultStatus<List<PetsViewModel>>(petInfoList, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<List<PetsViewModel>>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<List<PetsViewModel>>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Get All Pet Listing for ADMIN Buisness Logic
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<List<PetsViewModel>>> AdminPetInfos()
        {
            var response = new ServiceResponse<List<PetsViewModel>>();
            try
            {
                var petInfoList = _unitOfWork.PetRepository.GetPetAll();

                if (petInfoList != null)
                {
                    response = this.SetResultStatus<List<PetsViewModel>>(petInfoList, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<List<PetsViewModel>>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<List<PetsViewModel>>(null, Messages_Resources.Error, false);
            }
            return response;
        }


        /// <summary>
        /// Create New Pet Buisness Logic
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> CreateNewPet(PetsViewModel petData)
        {
            ServiceResponse<PetsViewModel> objReturn;
            try
            {
                //Save Data in Repo
                var responseId = await _unitOfWork.PetRepository.SavePetDataAsync(petData);

                if (!string.IsNullOrEmpty(responseId))
                {
                    petData.Id = Guid.Parse(responseId);
                    objReturn = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.PetAdded, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<PetsViewModel>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<PetsViewModel>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }


        /// <summary>
        /// Create New Pet Buisness Logic
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> UpdatePet(PetsViewModel petData)
        {
            ServiceResponse<PetsViewModel> objReturn;
            try
            {
                //Check Existing Pet Data
                PetsViewModel existingTempPet = _unitOfWork.PetRepository.GetPetDataByPetId(petData.Id);

                if (existingTempPet is null)
                    return this.SetResultStatus<PetsViewModel>(null, MessageStatus.PetNotExists, false);

                //Save Data in Repo
                var success = await _unitOfWork.PetRepository.UpdatePetDataAsync(petData);

                if (success)
                {
                    objReturn = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.PetUpdated, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<PetsViewModel>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<PetsViewModel>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }


        //Delete Pet Buisness Logic
        public async Task<ServiceResponse<string>> DeletePet(Guid petId)
        {
            ServiceResponse<string> response;
            try
            {
                var isSuccess = await _unitOfWork.PetRepository.DeletePetAsync(petId);

                if (isSuccess)
                {
                    DeletePetsOnAI(new List<Guid> { petId });
                    response = this.SetResultStatus<string>("Success", MessageStatus.PetDeleted, true);

                }
                else
                {
                    response = this.SetResultStatus<string>(null, MessageStatus.PetDeleteError, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Pet Detail By GUID pet Id
        /// </summary>
        /// <param name="petId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> petDetail(Guid petId)
        {
            var response = new ServiceResponse<PetsViewModel>();
            try
            {
                var petData = _unitOfWork.PetRepository.GetPetData(petId);

                if (petData != null)
                {
                    response = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Pet Detail by String Pet Id For ADMIN
        /// </summary>
        /// <param name="petId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> petDetail(string petId, string baseURL)
        {
            var response = new ServiceResponse<PetsViewModel>();
            try
            {
                var petData = _unitOfWork.PetRepository.GetPetDataByStringId(petId, baseURL);

                if (petData != null)
                {
                    response = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Pet Detail By microCip Number
        /// </summary>
        /// <param name="microNumber"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<PetsViewModel>> petDetailByMicroNumber(String microNumber)
        {
            var response = new ServiceResponse<PetsViewModel>();
            try
            {
                var petData = _unitOfWork.PetRepository.GetPetData(microNumber);

                if (petData != null)
                {
                    response = this.SetResultStatus<PetsViewModel>(petData, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<PetsViewModel>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Delete All Pets of User on Cancel or Expire Subscription
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productTitle"></param>
        /// <returns></returns>
        public bool DeleteAllPets(Guid userId, string productTitle)
        {
            if (!string.IsNullOrEmpty(productTitle))
            {
                var petAddCount = 0;

                if (productTitle.ToLower().Contains("multi pet"))
                {
                    petAddCount = 5;
                }
                else if (productTitle.ToLower().Contains("two pet"))
                {
                    petAddCount = 2;
                }
                else
                {
                    petAddCount = 1;
                }

                var allPets = _unitOfWork.PetRepository.GetAll().Where(a => a.UserID == userId).OrderByDescending(o => o.CreatedOn).Take(petAddCount);
                if (allPets.Any())
                {
                    var petIds = allPets.Select(s => s.Id).ToList();
                    var allMissingPets = _unitOfWork.MissingPetRepository.GetAll().Where(a => a.PetId != null && petIds.Contains(a.PetId.Value));
                    if (allMissingPets.Any())
                        _unitOfWork.MissingPetRepository.RemoveRange(allMissingPets);
                    _unitOfWork.PetRepository.RemoveRange(allPets);

                    //delete all the pets on AI
                    DeletePetsOnAI(allPets.Select(s => s.Id).ToList()).GetAwaiter().GetResult();
                    return true;
                }
            }
            else //delete all pets if product title will be null
            {
                var allPets = _unitOfWork.PetRepository.GetAll().Where(a => a.UserID == userId);
                if (allPets.Any())
                {
                    var petIds = allPets.Select(s => s.Id).ToList();
                    var allMissingPets = _unitOfWork.MissingPetRepository.GetAll().Where(a => a.PetId != null && petIds.Contains(a.PetId.Value));
                    if (allMissingPets.Any())
                        _unitOfWork.MissingPetRepository.RemoveRange(allMissingPets);
                    _unitOfWork.PetRepository.RemoveRange(allPets);

                    //delete all the pets on AI
                    DeletePetsOnAI(allPets.Select(s => s.Id).ToList()).GetAwaiter().GetResult();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check Similar Dog in the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> SimilarDogRequest(SimilarDogRequestViewModel model)
        {
            var dogReqUrl = configuataion["CustomKeys:DogRequestUrl"];
            var dogReqKey = configuataion["CustomKeys:DogRequestApiKey"];
            using var form = new MultipartFormDataContent();
            using (var ms = new MemoryStream())
            {
                var fileName = model.Image.FileName;
                model.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var fileContent = new ByteArrayContent(fileBytes);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                // here it is important that second parameter matches with name given in API.
                form.Add(fileContent, "nose_image", fileName);
            }

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(dogReqUrl)
            };
            httpClient.DefaultRequestHeaders.Add("X-API-Key", dogReqKey);
            var response = await httpClient.PostAsync($"similar", form);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        /// <summary>
        /// Analyze Dog Full Image for Breed and other details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        
        public async Task<string> AnalyzeDogRequest(AnalyzeDogRequestViewModel model)
        {
            var dogReqUrl = configuataion["CustomKeys:DogRequestUrl"];
            var dogReqKey = configuataion["CustomKeys:DogRequestApiKey"];
    
           

            using var form = new MultipartFormDataContent();

            using (var ms = new MemoryStream())
            {
                model.NoseImage.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var fileContent = new ByteArrayContent(fileBytes);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                var ext = Path.GetExtension(model.NoseImage.FileName);
                var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                form.Add(fileContent, "nose_image", $"{timestamp}_nose_image{ext}");
            }
            using (var ms = new MemoryStream())
            {
                model.DogImage.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var fileContent = new ByteArrayContent(fileBytes);
                var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                var ext = Path.GetExtension(model.DogImage.FileName);

                // here it is important that second parameter matches with name given in API.
                form.Add(fileContent, "dog_image", $"{timestamp}_dog_image{ext}");
            }



            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(dogReqUrl)
            };
            httpClient.DefaultRequestHeaders.Add("X-API-Key", dogReqKey);
            var response = await httpClient.PostAsync($"analyze", form);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }


        /// <summary>
        /// Register Dog to the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> RegisterDogRequest(RegisterDogRequestViewModel model)
        {
            var dogReqUrl = configuataion["CustomKeys:DogRequestUrl"];
            var dogReqKey = configuataion["CustomKeys:DogRequestApiKey"];

            using var form = new MultipartFormDataContent();

            using (var ms = new MemoryStream())
            {
                model.NoseImage.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var fileContent = new ByteArrayContent(fileBytes);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                var ext = Path.GetExtension(model.NoseImage.FileName);
                form.Add(fileContent, "nose_image", model.PetId + "_nose_image" + ext);
            }
            using (var ms = new MemoryStream())
            {
                model.DogImage.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var fileContent = new ByteArrayContent(fileBytes);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                var ext = Path.GetExtension(model.DogImage.FileName);
                // here it is important that second parameter matches with name given in API.
                form.Add(fileContent, "dog_image", model.PetId + "_dog_image" + ext);
            }

            form.AddParam("ds_id", model.PetId);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(dogReqUrl)
            };
            httpClient.DefaultRequestHeaders.Add("X-API-Key", dogReqKey);
            var response = await httpClient.PostAsync($"register", form);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }



        //delete the pets on AI
        public async Task<string> DeletePetsOnAI(List<Guid> petIds)
        {
            var dogReqUrl = configuataion["CustomKeys:DogRequestUrl"];
            var dogReqKey = configuataion["CustomKeys:DogRequestApiKey"];

            string commaSeparatedIds = string.Join(",", petIds.Select(x => x));

            using var form = new MultipartFormDataContent();
            form.AddParam("ds_ids", commaSeparatedIds);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(dogReqUrl)
            };
            httpClient.DefaultRequestHeaders.Add("X-API-Key", dogReqKey);
            var response = await httpClient.PostAsync($"delete_multiple", form);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        
    }
}
