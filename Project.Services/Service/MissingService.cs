using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Models.CommonModel;
using Project.Models.Pets;
using Project.Persistence.UOW;
using Project.Services.IService;
using Project.Services.Resources;
using Project.Services.ServiceEntities;
using ServiceStack.Html;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Service
{
    public class MissingService : BaseService, IMissingService
    {
        //UnitOfWork Accessing for DB Tables
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        public MissingService(IUnitOfWork unitOfWork, IExceptionLoggerService exceptionLoggerService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _exceptionLoggerService = exceptionLoggerService;
            _emailService = emailService;
        }

        public async Task<ServiceResponse<List<MissingPetsViewModel>>> AdminMissingPetInfos()
        {
            var response = new ServiceResponse<List<MissingPetsViewModel>>();
            try
            {
                var users = _unitOfWork.UserAccountRepository.GetUsers().ToList();
                var petInfoList = _unitOfWork.PetRepository.GetPetAll().Where(a => a.IsMissing);
                var missingInfoList = _unitOfWork.MissingPetRepository.GetAllMissingPet();
                var dd = petInfoList.Join(missingInfoList, p => p.Id, mp => mp.PetId, (p, mp) => new { p, mp })
                    .Join(users, pmj => pmj.p.PetOwnerId, user => user.Id, (pmj, user) => new { pmj, user })
                    .Select(a => new MissingPetsViewModel()
                    {
                        Id = a.pmj.p.Id,
                            PetId = a.pmj.mp.PetId,
                            PetTypeId = a.pmj.p.PetTypeId,
                            Name = a.pmj.p.PName,
                            ContactNo = a.pmj.p.ContactNumber,
                            Description = a.pmj.mp.Description,
                            Address = a.pmj.mp.Address,
                            OwnerName = a.user.FirstName + " " + a.user.LastName,
                            OwnerEmail = a.user.Email,
                            LostDate = a.pmj.mp.LostDate,
                            CreatedDate = a.pmj.mp.CreatedDate,

                        }).OrderByDescending(item => item.LostDate).ToList();
                if (dd != null)
                {
                    response = this.SetResultStatus<List<MissingPetsViewModel>>(dd, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<List<MissingPetsViewModel>>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<List<MissingPetsViewModel>>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        /// <summary>
        /// Found Missing Pet Entry
        /// </summary>
        /// <param name="messingDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> FoundMissingPetByAnonymous(FoundMissingPetRequest missingDetails)
        {
            ServiceResponse<string> objReturn;
            try
            {
                var success = false;
                // Validate input
                if (missingDetails == null)
                {
                    throw new ArgumentNullException(nameof(missingDetails), "Pet data cannot be null");
                }

                success = await _unitOfWork.MissingPetRepository.GuestUserPetFoundDataAsync(missingDetails);

                if (success)
                {
                    objReturn = this.SetResultStatus<string>("Success", MessageStatus.GuestUserFoundPetMessage, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<string>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<string>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }

        /// <summary>
        /// Found Missing Pet Entry
        /// </summary>
        /// <param name="messingDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> FoundMissingPet(FoundMissingPetRequest missingDetails)
        {
            ServiceResponse<string> objReturn;
            try
            {
                var success = false;
                // Validate input
                if (missingDetails == null)
                {
                    throw new ArgumentNullException(nameof(missingDetails), "Pet data cannot be null");
                }

                success = await _unitOfWork.MissingPetRepository.GuestUserPetFoundDataAsync(missingDetails);

                if (success)
                {
                    objReturn = this.SetResultStatus<string>("Success", MessageStatus.GuestUserFoundPetMessage, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<string>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<string>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }

        /// <summary>
        /// Found Missing Pet Entry
        /// </summary>
        /// <param name="messingDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> FoundMyPet(FoundMissingPetRequest missingDetails)
        {
            ServiceResponse<string> objReturn;
            try
            {
                var success = false;
                // Validate input
                if (missingDetails == null)
                {
                    throw new ArgumentNullException(nameof(missingDetails), "Pet data cannot be null");
                }

                success = await _unitOfWork.MissingPetRepository.PetFoundDataAsync(missingDetails);

                if (success)
                {
                    await _unitOfWork.PetRepository.UpdateIsMissingAsync(missingDetails.PetId, false);
                    objReturn = this.SetResultStatus<string>("Success", MessageStatus.FoundMessage, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<string>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<string>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }


        /// <summary>
        /// Report Missing Pet
        /// </summary>
        /// <param name="missingDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> ReportMissingPet(MissingPetRequestViewModel missingDetails, string userEmail)
        {
            ServiceResponse<string> objReturn;
            try
            {
                //Save Data in Repo

                if (missingDetails is null)
                    throw new ArgumentNullException(nameof(missingDetails), "data cannot be null");

                var missingPetData = await _unitOfWork.MissingPetRepository.GetMissingPet(missingDetails.PetId);
                var success = false;
                if (missingPetData != null)
                {
                    success = await _unitOfWork.MissingPetRepository.UpdateMissingPetPetDataAsync(missingDetails);
                }
                else
                {
                    success = await _unitOfWork.MissingPetRepository.AddMissingPet(missingDetails);
                }

                if (success)
                {
                    //Chnage the status of isMissing in pet Data
                    var pname = await _unitOfWork.PetRepository.UpdatePetIsMissingNReturnName(missingDetails.PetId, missingDetails.Status == PetStatus.Lost);
                    try
                    {
                        await _emailService.SendMissingPetAcknowledgeEmail(userEmail);
                        if (!string.IsNullOrEmpty(pname))
                            await _emailService.SendMissingPetSupportEmail(userEmail, pname);
                    }
                    catch { /* Email failure should not block a successful missing pet report */ }
                    objReturn = this.SetResultStatus<string>("Success", MessageStatus.MissingReportAdded, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<string>(null, MessageStatus.Error, false);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoggerService.LogException(ex);
                objReturn = this.SetResultStatus<string>(null, MessageStatus.Fail, false);
            }
            return objReturn;
        }

        public async Task<ServiceResponse<List<IDCheckViewModel>>> AdminIDCheckPets()
        {
            var response = new ServiceResponse<List<IDCheckViewModel>>();
            try
            {
                var petInfoList = _unitOfWork.PetRepository.GetPetAll().Where(a => a.IsMissing);
                var missingInfoList = _unitOfWork.MissingPetRepository.GetAllMissingPet().Where(a => a.FoundBy != null);
                var users = _unitOfWork.UserAccountRepository.GetUsers().ToList();
                var dd = petInfoList.Join(missingInfoList, p => p.Id, mp => mp.PetId, (p, mp) => new { p, mp })
                    .Join(_unitOfWork.UserAccountRepository.GetUsers(), pmj => pmj.p.PetOwnerId, user => user.Id, (pmj, user) => new { pmj, user })
                    .Join(_unitOfWork.UserAccountRepository.GetUsers(), pmjj => pmjj.pmj.mp.FoundBy, iduser => iduser.Id, (pmjj, iduser) => new { pmjj, iduser })
                    .Select(a => new IDCheckViewModel()
                    {
                        PetId = a.pmjj.pmj.p.Id,
                        MissingPetId = a.pmjj.pmj.mp.Id,
                        PetName = a.pmjj.pmj.p.PName,
                        PetOwnerName = a.pmjj.user.FirstName + " " + a.pmjj.user.LastName,
                        PetOwnerEmail = a.pmjj.user.Email,
                        PetOwnerContactNo = a.pmjj.user.PhoneNumber,
                        Description = a.pmjj.pmj.mp.Description,
                        Address = a.pmjj.pmj.mp.Address,
                        GuestName = a.iduser.FirstName + " " + a.iduser.LastName,
                        GuestContactNo = a.iduser.PhoneNumber,
                        GuestEmail = a.iduser.Email,
                        FoundBy = a.iduser.Id,
                        LostDate = a.pmjj.pmj.mp.LostDate

                    }).ToList();
                if (petInfoList != null)
                {
                    response = this.SetResultStatus<List<IDCheckViewModel>>(dd, MessageStatus.Success, true);

                }
                else
                {
                    response = this.SetResultStatus<List<IDCheckViewModel>>(null, Messages_Resources.NotExists, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<List<IDCheckViewModel>>(null, Messages_Resources.Error, false);
            }
            return response;
        }

    }
}
