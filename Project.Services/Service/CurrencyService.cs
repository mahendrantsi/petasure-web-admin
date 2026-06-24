using AutoMapper;
using SmartPay.Core.Extension;
using SmartPay.Data.DBEntities;
using SmartPay.Models.AdminModel;
using SmartPay.Models.CommonModel;
using SmartPay.Persistence.UOW;
using SmartPay.Services.IService;
using SmartPay.Services.ServiceEntities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.Service
{
    public class CurrencyService: BaseService, ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CurrencyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //public SelectList GetCurrency()
        //{
        //    var currencyList = this._unitOfWork.CurrencyRepository.GetAll().Where(x => x.IsCrypto == true);
        //    return new SelectList(currencyList, "Id", "Name");
        //}

        public async Task<ServiceResponse<CurrencyViewModel>> Create(CurrencyViewModel currencyViewModel)
        {
            ServiceResponse<CurrencyViewModel> objReturn = new ServiceResponse<CurrencyViewModel>();
            try
            {
                CurrencyMaster currency = this._mapper.Map<CurrencyViewModel, CurrencyMaster>(currencyViewModel);
                currency.CreatedOn = currency.ModifiedOn = DateTime.UtcNow;
                this._unitOfWork.CurrencyRepository.Add(currency);
                await this._unitOfWork.SaveChangesAsync();
                currencyViewModel.Id = currency.Id;
                objReturn = this.SetResultStatus<CurrencyViewModel>(currencyViewModel, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<CurrencyViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<CurrencyViewModel>> Edit(CurrencyViewModel currencyViewModel)
        {
            ServiceResponse<CurrencyViewModel> objReturn = new ServiceResponse<CurrencyViewModel>();
            try
            {
                var currency = await _unitOfWork.CurrencyRepository.GetByIdAsync(currencyViewModel.Id);
                currency.Name = currencyViewModel.Name;
                currency.Description = currencyViewModel.Description;
                currency.ModifiedOn = DateTime.UtcNow;
                currency.ModifiedBy = 1;
                currency.ImageURL = currencyViewModel.ImageURL;
                currency.IsActive = currencyViewModel.IsActive;

                this._unitOfWork.CurrencyRepository.UpdateEntity(currency);
                await this._unitOfWork.SaveChangesAsync();
                objReturn = this.SetResultStatus<CurrencyViewModel>(currencyViewModel, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<CurrencyViewModel>(null, MessageStatus.Error, false);
            }
            return objReturn;
        }

        public async Task<ServiceResponse<CurrencyViewModel>> GetById(long id)
        {
            ServiceResponse<CurrencyViewModel> objReturn = new ServiceResponse<CurrencyViewModel>();
            try
            {
                var obj = await this._unitOfWork.CurrencyRepository.GetByIdAsync(id);
                CurrencyViewModel currencyViewModel = _mapper.Map<CurrencyMaster, CurrencyViewModel>(obj);
                objReturn = this.SetResultStatus<CurrencyViewModel>(currencyViewModel, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<CurrencyViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }
        public async Task<ServiceResponse<List<CurrencyViewModel>>> GetAll(JQueryDataTableModel param)
        {
            ServiceResponse<List<CurrencyViewModel>> objReturn = new ServiceResponse<List<CurrencyViewModel>>();
            try
            {
                object[] paramObject = new object[5];
                paramObject[0] = param.start;
                paramObject[1] = param.length;
                paramObject[2] = param.ordercolumn;
                paramObject[3] = param.sortorder;
                paramObject[4] = !string.IsNullOrEmpty(param.search) ? param.search : null;
                var currencyListResults = await _unitOfWork.CurrencyRepository.GetAllCurrency(paramObject);
                var currencyViewModels = _mapper.Map<List<CurrencyViewModel>>(currencyListResults);
                if (currencyViewModels.Count > 0)
                {
                    objReturn = this.SetResultStatus<List<CurrencyViewModel>>(currencyViewModels, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<List<CurrencyViewModel>>(null, MessageStatus.NotExists, false);
                }
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<List<CurrencyViewModel>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }
    }
}
