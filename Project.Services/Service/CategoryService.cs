using AutoMapper;
using SmartPay.Core.Extension;
using SmartPay.Data.DBEntities;
using SmartPay.Models.AdminModel;
using SmartPay.Models.CommonModel;
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
    public class CategoryService: BaseService, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //public SelectList GetCurrency()
        //{
        //    var currencyList = this._unitOfWork.CategoryRepository.GetAll().Where(x => x.IsCrypto == true);
        //    return new SelectList(currencyList, "Id", "Name");
        //}

        public async Task<ServiceResponse<CategoryViewModel>> Create(CategoryViewModel categoryViewModel)
        {
            ServiceResponse<CategoryViewModel> objReturn = new ServiceResponse<CategoryViewModel>();
            try
            {
                CategoryMaster category = this._mapper.Map<CategoryViewModel, CategoryMaster>(categoryViewModel);
                category.CreatedOn = category.ModifiedOn = DateTime.UtcNow;
                this._unitOfWork.CategoryRepository.Add(category);
                await this._unitOfWork.SaveChangesAsync();
                categoryViewModel.Id = category.Id;
                objReturn = this.SetResultStatus<CategoryViewModel>(categoryViewModel, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<CategoryViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<CategoryViewModel>> Edit(CategoryViewModel categoryViewModel)
        {
            ServiceResponse<CategoryViewModel> objReturn = new ServiceResponse<CategoryViewModel>();
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryViewModel.Id);
                category.Name = categoryViewModel.Name;
                category.Description = categoryViewModel.Description;
                category.ModifiedOn = DateTime.UtcNow;
                category.ModifiedBy = 1;
                this._unitOfWork.CategoryRepository.UpdateEntity(category);
                await this._unitOfWork.SaveChangesAsync();
                objReturn = this.SetResultStatus<CategoryViewModel>(categoryViewModel, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<CategoryViewModel>(null, MessageStatus.Error, false);
            }
            return objReturn;
        }

        public async Task<ServiceResponse<CategoryViewModel>> GetById(long id)
        {
            ServiceResponse<CategoryViewModel> objReturn = new ServiceResponse<CategoryViewModel>();
            try
            {
                var obj = await this._unitOfWork.CategoryRepository.GetByIdAsync(id);
                CategoryViewModel categoryViewModel = _mapper.Map<CategoryMaster, CategoryViewModel>(obj);
                objReturn = this.SetResultStatus<CategoryViewModel>(categoryViewModel, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<CategoryViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }
        public async Task<ServiceResponse<List<CategoryViewModel>>> GetAll(JQueryDataTableModel param)
        {
            ServiceResponse<List<CategoryViewModel>> objReturn = new ServiceResponse<List<CategoryViewModel>>();
            try
            {
                object[] paramObject = new object[5];
                paramObject[0] = param.start;
                paramObject[1] = param.length;
                paramObject[2] = param.ordercolumn;
                paramObject[3] = param.sortorder;
                paramObject[4] = !string.IsNullOrEmpty(param.search) ? param.search : null;
                var categoryListResults = await _unitOfWork.CategoryRepository.GetAllCategories(paramObject);
                var categoryViewModels = _mapper.Map<List<CategoryViewModel>>(categoryListResults);
                if (categoryViewModels.Count > 0)
                {
                    objReturn = this.SetResultStatus<List<CategoryViewModel>>(categoryViewModels, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<List<CategoryViewModel>>(null, MessageStatus.NotExists, false);
                }
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<List<CategoryViewModel>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }
    }
}
