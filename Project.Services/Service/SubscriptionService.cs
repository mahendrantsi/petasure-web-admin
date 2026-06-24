using Castle.Core.Resource;
using Project.Core.Enum;
using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Models.Pets;
using Project.Models.Subscription;
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
    public class SubscriptionService : BaseService, ISubscriptionService
    {
        //UnitOfWork Accessing for DB Tables
        private readonly IUnitOfWork _unitOfWork;

        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IPetService _petService;

        public SubscriptionService(IUnitOfWork unitOfWork, IExceptionLoggerService exceptionLoggerService,
            IPetService petService)
        {
            _unitOfWork = unitOfWork;
            _exceptionLoggerService = exceptionLoggerService;
            _petService = petService;
        }

        public async Task<ServiceResponse<List<ApiSubscriptionResponseModel>>> GetActiveSubscriptionList(Guid userID, EnumUserType userType)
        {
            var response = new ServiceResponse<List<ApiSubscriptionResponseModel>>();
            try
            {
                if (userType == Core.Enum.EnumUserType.User)
                {
                    var subscriptionList = _unitOfWork.SubscriptionRepository.GetSubscriptionAll(userID).Where(a => a.Status.ToLower() == "active").ToList();

                    if (subscriptionList.Any())
                    {
                        response = this.SetResultStatus<List<ApiSubscriptionResponseModel>>(subscriptionList.Select(s => new ApiSubscriptionResponseModel()
                        {
                            SubscriptionId = s.SubscriptionId,
                            NextChargeScheduleOn = s.NextChargeScheduleOn,
                            ProductTitle = s.ProductTitle,
                            Status = s.Status

                        }).ToList(), MessageStatus.Success, true);
                    }
                    else
                    {
                        response = this.SetResultStatus<List<ApiSubscriptionResponseModel>>(new List<ApiSubscriptionResponseModel>(), Messages_Resources.NotActiveSubscription, false);
                    }
                }
                else
                {
                    var subscriptionList = _unitOfWork.InAppPurchaseRepository.GetInAppPurchases(userID).ToList();

                    if (subscriptionList.Any())
                    {
                        response = this.SetResultStatus<List<ApiSubscriptionResponseModel>>(subscriptionList.Select(s => new ApiSubscriptionResponseModel()
                        {
                            ProductTitle = s.ProductTitle,
                            ProductId = s.ProductId,
                            NextChargeScheduleOn = s.ExpireDate

                        }).ToList(), MessageStatus.Success, true);
                    }
                    else
                    {
                        response = this.SetResultStatus<List<ApiSubscriptionResponseModel>>(new List<ApiSubscriptionResponseModel>(), Messages_Resources.NotActiveSubscription, false);
                    }
                }
            }
            catch (Exception ex)
            {
                response = this.SetResultStatus<List<ApiSubscriptionResponseModel>>(new List<ApiSubscriptionResponseModel>(), Messages_Resources.Error, false);
            }
            return response;
        }

        public async Task<ServiceResponse<ApiSubscriptionResponseModel>> GetActiveSubscriptions(Guid userID)
        {
            var response = new ServiceResponse<ApiSubscriptionResponseModel>();
            try
            {
                var subscriptionList = _unitOfWork.SubscriptionRepository.GetSubscriptionAll(userID).Where(a => a.Status.ToLower() == "active").ToList();
                var maxNextCharge = subscriptionList.FirstOrDefault(a => a.NextChargeScheduleOn == subscriptionList.Max(a => a.NextChargeScheduleOn));
                if (maxNextCharge != null)
                {
                    response = this.SetResultStatus<ApiSubscriptionResponseModel>(new ApiSubscriptionResponseModel()
                    {
                        SubscriptionId = maxNextCharge.SubscriptionId,
                        NextChargeScheduleOn = maxNextCharge.NextChargeScheduleOn,
                        ProductTitle = maxNextCharge.ProductTitle,
                        Status = maxNextCharge.Status

                    }, MessageStatus.Success, true);
                }
                else
                {
                    response = this.SetResultStatus<ApiSubscriptionResponseModel>(new ApiSubscriptionResponseModel(), Messages_Resources.NotActiveSubscription, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<ApiSubscriptionResponseModel>(null, Messages_Resources.Error, false);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> SaveSubscription(SubscriptionViewModel data)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = _unitOfWork.SubscriptionRepository.IsSubscriptionExist(data.SubscriptionId);
                if (existing == false)
                {
                    _unitOfWork.SubscriptionRepository.SaveSubscription(data);
                    response = this.SetResultStatus<string>("Success", MessageStatus.Success, true);
                }

                response = this.SetResultStatus<string>("Fail", MessageStatus.Fail, false);
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>("Exception", MessageStatus.Fail, false);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> ActivateSubscription(SubscriptionViewModel model)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = _unitOfWork.SubscriptionRepository.IsSubscriptionExist(model.SubscriptionId);
                if (existing == true)
                {
                    _unitOfWork.SubscriptionRepository.UpdateSubscriptionOnActivation(model);
                    response = this.SetResultStatus<string>("Success", MessageStatus.Success, true);
                }

                response = this.SetResultStatus<string>("Fail", MessageStatus.Fail, false);
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>("Exception", MessageStatus.Fail, false);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> CancelSubscription(SubscriptionViewModel model)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = _unitOfWork.SubscriptionRepository.GetById(model.SubscriptionId);
                if (existing != null)
                {
                    var user = _unitOfWork.UserAccountRepository.GetUsers().FirstOrDefault(a => a.ShopifyId == existing.CustomerId);
                    _petService.DeleteAllPets(user.Id, existing.ProductTitle);
                    _unitOfWork.SubscriptionRepository.UpdateSubscriptionOnCancelled(model);
                    response = this.SetResultStatus<string>("Success", MessageStatus.Success, true);
                }

                response = this.SetResultStatus<string>("Fail", MessageStatus.Fail, false);
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>("Exception", MessageStatus.Fail, false);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> SkippedSubscription(SubscriptionViewModel model)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = _unitOfWork.SubscriptionRepository.IsSubscriptionExist(model.SubscriptionId);
                if (existing == true)
                {
                    _unitOfWork.SubscriptionRepository.UpdateSubscriptionOnSkipped(model);
                    response = this.SetResultStatus<string>("Success", MessageStatus.Success, true);
                }

                response = this.SetResultStatus<string>("Fail", MessageStatus.Fail, false);
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>("Exception", MessageStatus.Fail, false);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> UnSkippedSubscription(SubscriptionViewModel model)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = _unitOfWork.SubscriptionRepository.IsSubscriptionExist(model.SubscriptionId);
                if (existing == true)
                {
                    _unitOfWork.SubscriptionRepository.UpdateSubscriptionOnUnSkipped(model);
                    response = this.SetResultStatus<string>("Success", MessageStatus.Success, true);
                }

                response = this.SetResultStatus<string>("Fail", MessageStatus.Fail, false);
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>("Exception", MessageStatus.Fail, false);
            }
            return response;
        }

        public async Task<bool> DeleteAllSubscriptions(int customerId)
        {
            try
            {
                var allSubscriptions = _unitOfWork.SubscriptionRepository.GetAll().Where(a => a.CustomerId == customerId);
                if (allSubscriptions.Any())
                    _unitOfWork.SubscriptionRepository.RemoveRange(allSubscriptions);
                return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        public async Task<ServiceResponse<string>> UpdateSubscription(SubscriptionViewModel model)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = _unitOfWork.SubscriptionRepository.IsSubscriptionExist(model.SubscriptionId);
                if (existing == true)
                {
                    _unitOfWork.SubscriptionRepository.UpdateSubscriptionOnUnSkipped(model);
                    response = this.SetResultStatus<string>("Success", MessageStatus.Success, true);
                }

                response = this.SetResultStatus<string>("Fail", MessageStatus.Fail, false);
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>("Exception", MessageStatus.Fail, false);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> SaveInAppPurchase(InAppPurchaseInputViewModel data)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = _unitOfWork.InAppPurchaseRepository.IsExist(data.ProductId, data.AspnetuserId);
                if (existing == false)
                {
                    await _unitOfWork.InAppPurchaseRepository.SaveInAppPurchase(data);
                }
                else
                    await _unitOfWork.InAppPurchaseRepository.UpdateInAppPurchase(data);
                response = this.SetResultStatus<string>("Success", MessageStatus.Success, true);
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>("Exception", MessageStatus.Fail, false);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> IsCertificateValid(Guid userid, bool isSandBox)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = _unitOfWork.InAppPurchaseRepository.IsCertificateValid(userid, isSandBox);
                if (existing.isvalid)
                {
                    response = this.SetResultStatus<string>("Success", MessageStatus.Success, true);
                }
                else
                {
                    _petService.DeleteAllPets(userid, existing.productTitle);
                    response = this.SetResultStatus<string>("Fail", MessageStatus.Expired, false);
                }
            }
            catch (Exception ex)
            {

                response = this.SetResultStatus<string>("Exception", MessageStatus.Fail, false);
            }
            return response;
        }
    }
}
