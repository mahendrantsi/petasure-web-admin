namespace SmartPay.Services.Service
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using AutoMapper;
    using SmartPay.Core.Extension;
    using SmartPay.Data.ExtendedDBEntities;
    using SmartPay.Models.CommonModel;
    using SmartPay.Models.GeneralModel;
    using SmartPay.Persistence.UOW;
    using SmartPay.Services.IService;
    using SmartPay.Services.ServiceEntities;

    public class UserRefrenceService : BaseService, IUserRefrenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserRefrenceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // public async Task<ServiceResponse<UserRefrenceAccountViewModel>> Create(string aspUserId, RegisterViewModel registerViewModel)
        // {
        //     ServiceResponse<UserRefrenceAccountViewModel> objReturn = new ServiceResponse<UserRefrenceAccountViewModel>();
        //     UserRefrenceAccountViewModel userRefrenceViewModel = new UserRefrenceAccountViewModel();
        //     try
        //     {
        //         object[] param = new object[4];
        //         param[0] = aspUserId;
        //         param[1] = registerViewModel.FirstName;
        //         param[2] = registerViewModel.LastName;
        //         param[3] = registerViewModel.PhoneNumber;
        //         UserRefrenceAccount userRefrence = await _unitOfWork.UserRefrenceRepository.CreateUserRefrence(aspUserId,  param);
        //         userRefrenceViewModel = _mapper.Map<UserRefrenceAccountViewModel>(userRefrence);
        //         if (userRefrenceViewModel.CustomerId > 0 && userRefrenceViewModel.AccountId > 0 && userRefrenceViewModel.AccountUserRefrenceId > 0)
        //         {
                    
        //             objReturn = this.SetResultStatus<UserRefrenceAccountViewModel>(userRefrenceViewModel, MessageStatus.Success, true);
        //         }
        //         else
        //         {
        //             objReturn = this.SetResultStatus<UserRefrenceAccountViewModel>(null, MessageStatus.Error, false);
        //         }
        //     }
        //     catch (Exception Ex)
        //     {
        //         objReturn = this.SetResultStatus<UserRefrenceAccountViewModel>(null, MessageStatus.Error, false);
        //     }

        //     return objReturn;
        // }

        // public async Task<ServiceResponse<UserInformationModel>> GetUserInformationByEmail(string email)
        // {
        //     ServiceResponse<UserInformationModel> objReturn = new ServiceResponse<UserInformationModel>();
        //     UserInformationModel userInformationModel = new UserInformationModel();
        //     try
        //     {
        //         var userInformation = await this._unitOfWork.UserRefrenceRepository.GetUserInfo(email);
        //         userInformationModel = this._mapper.Map<UserInformation, UserInformationModel>(userInformation);
        //         if (userInformationModel != null)
        //         {
        //             objReturn = this.SetResultStatus<UserInformationModel>(userInformationModel, MessageStatus.Success, true);
        //         }
        //         else
        //         {
        //             objReturn = this.SetResultStatus<UserInformationModel>(null, MessageStatus.Error, false);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         objReturn = this.SetResultStatus<UserInformationModel>(null, MessageStatus.Error, false);
        //     }

        //     return objReturn;
        // }
        // public async Task<ServiceResponse<UserAccountViewModel>> GetAllUsers(IndexModel indexModel)
        // {
        //     var response = new ServiceResponse<UserAccountViewModel>();
        //     try
        //     {
        //         object[] param = new object[4];
        //         param[0] = indexModel.OrderBy;
        //         param[1] = indexModel.Page;
        //         param[2] = indexModel.PageSize;
        //         UserAccount userRefrence = await this._unitOfWork.UserRefrenceRepository.GetAllUsers(param);
        //         userRefrenceViewModel = _mapper.Map<UserRefrenceAccountViewModel>(userRefrence);
        //     }
        //     catch (Exception ex)
        //     {
        //         response = SetResultStatus<UserAccountViewModel>(null, MessageStatus.Error, false);
        //     }
        //     return response;
        // }
    }
}
