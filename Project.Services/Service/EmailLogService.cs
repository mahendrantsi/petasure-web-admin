namespace Project.Services.Service
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Project.Core.Extension;
    using Project.Data.DBEntities;
    using Project.Models.CommonModel;
    using Project.Persistence.UOW;
    using Project.Services.IService;
    using Project.Services.ServiceEntities;

    public class EmailLogService : BaseService, IEmailLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmailLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<EmailLogViewModel>> Create(EmailLogViewModel emailLogViewModel)
        {
            ServiceResponse<EmailLogViewModel> objReturn = new ServiceResponse<EmailLogViewModel>();
            try
            {
                var emailLog = this._mapper.Map<EmailLogViewModel, EmailLog>(emailLogViewModel);
                this._unitOfWork.EmailLogRepository.Add(emailLog);
                 this._unitOfWork.SaveChanges();
                emailLogViewModel.Id = emailLog.Id;
                objReturn = this.SetResultStatus<EmailLogViewModel>(emailLogViewModel, MessageStatus.Success, true);
            }
            catch(Exception ex)
            {
                objReturn = this.SetResultStatus<EmailLogViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }
    }
}
