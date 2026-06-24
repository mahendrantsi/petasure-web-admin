namespace SmartPay.Services.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using SmartPay.Core.Enum;
    using SmartPay.Core.Extension;
    using SmartPay.Data;
    using SmartPay.Data.DBEntities;
    using SmartPay.Models.CommonModel;
    using SmartPay.Models.FeeModel;
    using SmartPay.Services.IService;
    using SmartPay.Services.ServiceEntities;
    using SmartPay.Persistence.UOW;
    using SmartPay.Data.ExtendedDBEntities;

    public class FeeService : BaseService, IFeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        /// <summary>
        /// Save FeeMastersAsync.
        /// </summary>
        /// <param name="feeMasterViewModel">Fee master Model.</param>
        /// <returns>Added model.</returns>
        public async Task<ServiceResponse<FeeMasterViewModel>> AddFeeMastersAsync(FeeMasterViewModel feeMasterViewModel)
        {
            bool status = false;
            ServiceResponse<FeeMasterViewModel> objReturn = new ServiceResponse<FeeMasterViewModel>();
            //List<FeeRangeMaster> feeRangeMaster = new List<FeeRangeMaster>();
            try
            {
                feeMasterViewModel.CreatedOn = DateTime.UtcNow;
                feeMasterViewModel.IsActive = true;
                FeeMaster feeMaster = _mapper.Map<FeeMaster>(feeMasterViewModel);
                
                _unitOfWork.GenericRepository<FeeMaster>().Add(feeMaster);
                status = await _unitOfWork.SaveChangesAsync();
                if (status)
                {
                    if (feeMasterViewModel.feeRangeMasterViewModel.Count() > 0)
                    {
                        List<FeeRangeMaster> feeRangeList = _mapper.Map<List<FeeRangeMaster>>(feeMasterViewModel.feeRangeMasterViewModel);
                        feeRangeList.ForEach(f =>
                        {
                            f.FeeId = Convert.ToInt32(feeMaster.Id);
                            f.CreatedOn = feeMasterViewModel.CreatedOn;
                            f.CreatedBy = feeMasterViewModel.CreatedBy;
                        });
                        _unitOfWork.GenericRepository<FeeRangeMaster>().AddRange(feeRangeList);
                        status = await _unitOfWork.SaveChangesAsync();
                    }
                }

                objReturn = SetResultStatus<FeeMasterViewModel>(feeMasterViewModel, status == true ? MessageStatus.Success : MessageStatus.Error, status);
            }
            catch
            {
                objReturn = SetResultStatus<FeeMasterViewModel>(null, MessageStatus.Error, status);
            }

            return objReturn;
        }

        /// <summary>
        /// Save EditFeeMastersAsync.
        /// </summary>
        /// <param name="feeMasterViewModel">Fee master Model.</param>
        /// <returns>Updated model.</returns>
        public async Task<ServiceResponse<FeeMasterViewModel>> AddEditFeeMastersAsync(FeeMasterViewModel feeMasterViewModel)
        {
            bool status = false;
            ServiceResponse<FeeMasterViewModel> objReturn = new ServiceResponse<FeeMasterViewModel>();
            // List<FeeRangeMaster> feeRangeMaster = new List<FeeRangeMaster>();
            try
            {
                var feeResult = this._unitOfWork.GenericRepository<FeeMaster>().Get(x => x.Id == feeMasterViewModel.Id).FirstOrDefault();
                feeResult.FeeTypeId = feeMasterViewModel.FeeTypeId;
                feeResult.Name = feeMasterViewModel.Name;
                feeResult.Description = feeMasterViewModel.Description;
                feeResult.TransactionType = feeMasterViewModel.TransactionType;
                feeResult.DefaultFeeAmount = feeMasterViewModel.DefaultFeeAmount ?? 0;
                feeResult.ModifiedOn = DateTime.UtcNow;
                feeResult.ModifiedBy = feeMasterViewModel.ModifiedBy;
                this._unitOfWork.GenericRepository<FeeMaster>().UpdateEntity(feeResult);
                status = await this._unitOfWork.SaveChangesAsync();
                var feeRangeResult = this._unitOfWork.GenericRepository<FeeRangeMaster>().Get(x => x.FeeId == feeResult.Id);
                if (feeRangeResult != null && feeRangeResult.Count() > 0)
                {
                    this._unitOfWork.GenericRepository<FeeRangeMaster>().RemoveRange(feeRangeResult);
                    await this._unitOfWork.SaveChangesAsync();
                }

                if (feeMasterViewModel.feeRangeMasterViewModel.Count() > 0)
                {
                    List<FeeRangeMaster> feeRangeList = _mapper.Map<List<FeeRangeMaster>>(feeMasterViewModel.feeRangeMasterViewModel);
                    feeRangeList.ForEach(f =>
                    {
                        f.FeeId = Convert.ToInt32(feeResult.Id);
                        f.CreatedOn = feeMasterViewModel.CreatedOn;
                        f.CreatedBy = feeMasterViewModel.CreatedBy;
                    });
                    _unitOfWork.GenericRepository<FeeRangeMaster>().AddRange(feeRangeList);
                    await this._unitOfWork.SaveChangesAsync();
                }

                objReturn = SetResultStatus<FeeMasterViewModel>(feeMasterViewModel, status == true ? MessageStatus.Update : MessageStatus.Error, status);
            }
            catch
            {
                objReturn = SetResultStatus<FeeMasterViewModel>(null, MessageStatus.Error, status);
            }

            return objReturn;
        }

        /// <summary>
        /// Get FeeMaterAsync By Id for Edit.
        /// </summary>
        /// <param name="id">Fee Id.</param>
        /// <returns>Return Fee model.</returns>
        public async Task<ServiceResponse<FeeMasterViewModel>> EditFeeMaterAsync(long id)
        {
            ServiceResponse<FeeMasterViewModel> objReturn = new ServiceResponse<FeeMasterViewModel>();
            FeeMasterViewModel feeModel = new FeeMasterViewModel();
            List<FeeRangeMasterViewModel> feeRangeModel = new List<FeeRangeMasterViewModel>();
            try
            {
                var feeResult = _unitOfWork.GenericRepository<FeeMaster>().Get(x => x.Id == id).FirstOrDefault();
                feeModel = _mapper.Map<FeeMasterViewModel>(feeResult);
                if (feeModel != null)
                {
                    feeModel.DefaultFeeAmount = Math.Round(feeResult.DefaultFeeAmount);
                    var feeRangeResult = _unitOfWork.GenericRepository<FeeRangeMaster>().Get(x => x.FeeId == feeModel.Id).ToList();
                    feeRangeModel = _mapper.Map<List<FeeRangeMasterViewModel>>(feeRangeResult);
                    if (feeRangeModel.Count() > 0)
                    {
                        feeRangeModel.ForEach(f =>
                        {
                            f.FromAmount = Math.Round(f.FromAmount ?? 0, 2);
                            f.ToAmount = Math.Round(f.ToAmount ?? 0, 2);
                            f.Fee = Math.Round(f.Fee ?? 0, 2);
                        });
                        feeModel.feeRangeMasterViewModel = feeRangeModel;
                    }
                    //else
                    //{
                    //    feeModel.feeRangeMasterViewModel = new List<FeeRangeMasterViewModel>();
                    //    feeModel.feeRangeMasterViewModel.Add(new FeeRangeMasterViewModel { FromAmount = null });
                    //}
                }

                objReturn = SetResultStatus<FeeMasterViewModel>(feeModel, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = SetResultStatus<FeeMasterViewModel>(feeModel, MessageStatus.Error, false);
            }

            return objReturn;
        }

        /// <summary>
        /// GetFeeTypes Method.
        /// </summary>
        /// <returns>Fee Types List.</returns>
        public SelectList GetFeeTypes()
        {
            try
            {
                var feeTypeList = this._unitOfWork.GenericRepository<FeeTypeMaster>().GetAll().ToList();
                return new SelectList(feeTypeList, "Id", "Name");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// GetTransactionTypes Method.
        /// </summary>
        /// <returns>Transaction Type List.</returns>
        public SelectList GetTransactionTypes()
        {
            //var transactionType = Enum.GetValues(typeof(EnumTransactionType)).Cast<EnumTransactionType>().Select(v => new SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() })
            //  .ToList();
            var transactionType = this._unitOfWork.GenericRepository<TransactionTypeMaster>().GetAll().Select(v => new SelectListItem { Text = v.Name, Value = v.Id.ToString() }).ToList();
            return new SelectList(transactionType, "Value", "Text");
        }


        /// <summary>
        /// GetFeeListData.
        /// </summary>
        /// <param name="requestParam">Requested Input Parameters.</param>
        /// <returns>Fee List.</returns>
        public ServiceResponse<List<FeeListViewModel>> GetFeeListData(JQueryDataTableModel requestParam)
        {
            ServiceResponse<List<FeeListViewModel>> objReturn = new ServiceResponse<List<FeeListViewModel>>();
            List<FeeListViewModel> feeListModel = new List<FeeListViewModel>();
            try
            {
                feeListModel = FeeList(requestParam);
                var propertyInfo = typeof(FeeListViewModel).GetProperty(requestParam.ordercolumn);
                objReturn = this.SetResultStatus<List<FeeListViewModel>>(feeListModel, MessageStatus.Success, true);
                (objReturn.Data, objReturn.recordsTotal, objReturn.recordsFiltered) = DataTableShorting<FeeListViewModel>(feeListModel, requestParam, propertyInfo);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<List<FeeListViewModel>>(null, MessageStatus.Error, false);
            }

            return objReturn;

        }

        /// <summary>
        /// FeeList Method for inteenal call.
        /// </summary>
        /// <param name="param">Requested Input Parameters.</param>
        /// <param name="userId">User Id.</param>
        /// <param name="feeId">Fee Id.</param>
        /// <returns>Fee List.</returns>
        private List<FeeListViewModel> FeeList(JQueryDataTableModel param, long userId = 0, long feeId = 0)
        {
            List<FeeListViewModel> feeList = new List<FeeListViewModel>();
            try
            {
                feeList = _unitOfWork.Instance.FeeMaster.Include(x => x.FeeTypeMaster).Select(x =>
                     new FeeListViewModel
                     {
                         Id = x.Id,
                         Name = x.Name,
                         Description = x.Description,
                         FeeType = x.FeeTypeMaster.Name,
                         //StrCreatedOn = x.CreatedOn.ToString("dd-MM-yyyy"),
                         StrCreatedOn = x.CreatedOn.ToString(),
                         IsActive = x.IsActive,
                         CreatedBy = x.CreatedBy,
                         DefaultFeeAmount = 0,
                         //CreatedByName = _unitOfWork.Instance.Set<DerivedIdentityUser>().Join(_unitOfWork.Instance.UserProfile, c =>
                         // c.Id, y => y.UserId, (c, y) => new { c, y }).Where(f => f.c.Id == x.CreatedBy).Select(s => (s.y.FirstName + " " + s.y.LastName)).FirstOrDefault(),
                         feeRangeMasterViewModels = _unitOfWork.Instance.FeeRangeMaster.Where(r => r.FeeId == x.Id).Select(
                             f => new FeeRangeMasterViewModel
                             {
                                 FeeId = f.FeeId.Value,
                                 Fee = f.Fee,
                                 FromAmount = f.FromAmount,
                                 ToAmount = f.ToAmount,

                             }).ToList(),
                         TransactionTypeName = _unitOfWork.Instance.Set<TransactionTypeMaster>().Join(_unitOfWork.Instance.FeeMaster, c => c.Id, y => y.TransactionType, (c, y) => new { c, y }).Where(f => f.c.Id == x.TransactionType).Select(s => s.c.Name).FirstOrDefault()
                     }).Where(x => x.IsActive == true && (userId == 0 || x.CreatedBy == userId) && (string.IsNullOrEmpty(param.search) || x.Name.ToLower().Contains(param.search) ||
                       x.Description.ToLower().Contains(param.search)) && (feeId == 0 || x.Id == feeId)).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return feeList;
        }

        /// <summary>
        /// GetFeeById Method for detail.
        /// </summary>
        /// <param name="id">Fee Id.</param>
        /// <returns>Fee Detail.</returns>
        public ServiceResponse<FeeListViewModel> GetFeeById(long id)
        {
            ServiceResponse<FeeListViewModel> objReturn = new ServiceResponse<FeeListViewModel>();
            List<FeeListViewModel> feeListModel = new List<FeeListViewModel>();
            JQueryDataTableModel requestParam;
            try
            {
                requestParam = new JQueryDataTableModel { search = "" };
                //feeListModel = FeeList(requestParam, 0, id);
                if (feeListModel.Count() > 0)
                {
                    objReturn = this.SetResultStatus<FeeListViewModel>(feeListModel.FirstOrDefault(), MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<FeeListViewModel>(new FeeListViewModel(), MessageStatus.NotExists, true);
                }
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<FeeListViewModel>(null, MessageStatus.Fail, true);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<FeeMaster>> getFeeMasterByTransactionType(int TransactionTypeId)
        {
            ServiceResponse<FeeMaster> objReturn = new ServiceResponse<FeeMaster>();
            try
            {
                var feeMaster = _unitOfWork.FeeMasterRepository.Find(x => x.TransactionType == TransactionTypeId).FirstOrDefault();

                objReturn = SetResultStatus<FeeMaster>(feeMaster, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = SetResultStatus<FeeMaster>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

    }
}
