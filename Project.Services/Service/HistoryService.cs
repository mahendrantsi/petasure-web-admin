using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.CommonModel;
using Project.Persistence.UOW;
using Project.Services.IService;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Service
{
    public class HistoryService: BaseService, IHistoryService
    {
        private readonly UserManager<DerivedIdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HistoryService(UserManager<DerivedIdentityUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<ServiceResponse<UserHistoryViewModel>> SaveUserHistory(UserRegister model)
        {
            ServiceResponse<UserHistoryViewModel> objReturn;
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                var data = _mapper.Map<UserHistory>(user);
                this._unitOfWork.UserHistoryRepository.Add(data);
                this._unitOfWork.Save();
                var res = _mapper.Map<UserHistoryViewModel>(model);
                objReturn = this.SetResultStatus<UserHistoryViewModel>(res, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<UserHistoryViewModel>(null, MessageStatus.Fail, true);
            }
            return objReturn;
        }

        public async Task<ServiceResponse<UserHistoryViewModel>> SaveUserProfileHistory(UserRegister model, Guid userId)
        {
            ServiceResponse<UserHistoryViewModel> objReturn;
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                var profile = this._unitOfWork.UserProfileRepository.SingleOrDefault(x => x.UserId == model.Id);
                var data = _mapper.Map<UserHistory>(user);
                data.FirstName = model.FirstName;
                data.LastName = model.LastName;
                data.Address = model.Address;
                data.BusinessName = model.BusinessName;
                data.PostCode = model.PostCode;
                data.PhoneNumber = model.PhoneNumber;
                data.ProfleUserID = model.UserID;
                data.UserProfileId = profile.Id;
                data.IsMerchant = model.IsMerchant;
                data.FCMToken = profile.FCMToken;
                data.IsActive = model.Active;
                data.Email = model.Email;
               
                data.PasswordHash = user.PasswordHash;
                data.DateOfBirth = model.DateOfBirth;
                data.TwoFactorEnabled = model.TwoFactorEnabled;
                data.IsDeviceConnected = model.IsDeviceConnected;
                data.Town = model.Town;
                data.Country = model.Country;
                data.CreatedBy = userId;
                data.CreatedOn = DateExtension.GetUtcDateTime;

                this._unitOfWork.UserHistoryRepository.Add(data);
                this._unitOfWork.Save();
                var res = _mapper.Map<UserHistoryViewModel>(model);
                objReturn = this.SetResultStatus<UserHistoryViewModel>(res, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<UserHistoryViewModel>(null, MessageStatus.Fail, true);
            }
            return objReturn;
        }


    }
}
