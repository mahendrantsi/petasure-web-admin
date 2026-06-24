using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Models.CommonModel;
using Project.Persistence.UOW;
using Project.Services.IService;
using Project.Services.Resources;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Service
{
    public class RegistrationServiceOTP:BaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;

        private readonly int otpExpireTime = 1;
        public RegistrationServiceOTP(IUnitOfWork unitOfWork, IExceptionLoggerService exceptionLoggerService, IEmailService _emailService, INotificationService notificationService)
        {
            this._unitOfWork = unitOfWork;
            this._exceptionLoggerService = exceptionLoggerService;
            this._emailService = _emailService;
            _notificationService = notificationService;
        }

       
    }
}
