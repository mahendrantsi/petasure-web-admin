using AutoMapper;
using SmartPay.Core.Extension;
using SmartPay.Data.DBEntities;
using SmartPay.Models.Master;
using SmartPay.Persistence.UOW;
using SmartPay.Services.IService;
using SmartPay.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.Service
{
    public class MasterService : BaseService, IMasterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MasterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<CategoryDTO>>> getCategories()
        {
            ServiceResponse<List<CategoryDTO>> objReturn = new ServiceResponse<List<CategoryDTO>>();
            try
            {
                var categories = this._unitOfWork.CategoryRepository.GetAll().ToList();
                if (categories.Count > 0)
                {
                    var CategoriesDTO = this._mapper.Map<List<CategoryMaster>, List<CategoryDTO>>(categories);
                    objReturn = this.SetResultStatus<List<CategoryDTO>>(CategoriesDTO, MessageStatus.Success, true);
                }
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<List<CategoryDTO>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<List<CurrencyDTO>>> getCurrencies()
        {
            ServiceResponse<List<CurrencyDTO>> objReturn = new ServiceResponse<List<CurrencyDTO>>();
            try
            {
                var currencies = this._unitOfWork.CurrencyRepository.Get(x => x.IsBaseCurrency == true).ToList();
                if (currencies.Count > 0)
                {
                    var CurrenciesDTO = this._mapper.Map<List<CurrencyMaster>, List<CurrencyDTO>>(currencies);
                    objReturn = this.SetResultStatus<List<CurrencyDTO>>(CurrenciesDTO, MessageStatus.Success, true);
                }
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<List<CurrencyDTO>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }
    }
}
