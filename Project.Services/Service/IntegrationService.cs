using AutoMapper;
using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.Master;
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
    public class IntegrationService : BaseService, IIntegrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IntegrationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IntegrationViewModel>> Create(IntegrationViewModel model)
        {
            try
            {
                var response  = await _unitOfWork.IntegrationRepository.CreateIntegration(model);
                if (response.Success)
                    return SetResultStatus<IntegrationViewModel>(model, response.Message, true);
                else

                    return SetResultStatus<IntegrationViewModel>(model, response.Message, false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceResponse<IntegrationViewModel>> Delete(Guid Id)
        {
            try
            {
                var obj = await _unitOfWork.GenericRepository<Integration>().GetByIdAsync(Id);

                if (obj != null)
                {
                    obj.IsActive = false;
                    _unitOfWork.GenericRepository<Integration>().UpdateEntity(obj);
                    await _unitOfWork.SaveChangesAsync();
                    return SetResultStatus<IntegrationViewModel>(null, MessageStatus.Delete, true);
                }
                else
                    return SetResultStatus<IntegrationViewModel>(null, MessageStatus.NotExists, false);
            }
            catch (Exception)
            {
                return SetResultStatus<IntegrationViewModel>(null, MessageStatus.Error, false);
            }
        }

        public ServiceResponse<List<IntegrationViewModel>> Get()
        {
            try
            {
                var response = _unitOfWork.IntegrationRepository.Get(x => x.IsActive);
                //if (response.Count()==0)
                //{
                //    throw new NotFiniteNumberException("test");
                //}
                return SetResultStatus(response.ToList(), MessageStatus.Success, true);
            }
            catch (NotFiniteNumberException ex)
            {
                return SetResultStatus<List<IntegrationViewModel>>(null, MessageStatus.NotExists, false);
            }
            catch (Exception ex)
            {
                return SetResultStatus<List<IntegrationViewModel>>(null, MessageStatus.Error, false);
            }
        } 
        public async Task<ServiceResponse<IntegrationViewModel>> Update(IntegrationViewModel model)
        {
            try
            {
                var result = await _unitOfWork.IntegrationRepository.UpdateIntegration(model);
                return SetResultStatus<IntegrationViewModel>(model, result.Message, result.Success);
            }
            catch (Exception ex)
            {
                return SetResultStatus<IntegrationViewModel>(model, MessageStatus.Error, false);
            }
        }
    }
}
