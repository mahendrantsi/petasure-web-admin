using Braintree;
using SmartPay.Core.Extension;
using SmartPay.Services.IService;
using SmartPay.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.Service
{
    public class PaymentService : BaseService, IPaymentService
    {
        private IBraintreeGateway BraintreeGateway { get; set; }

        private IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "4r49y4fznsxf9phg",
                PublicKey = "ypmvm9b9ygxbghsd",
                PrivateKey = "0ed95f07cfbc00907aa388ab6fbba4cf"
            };
        }

        private IBraintreeGateway GetGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }

            return BraintreeGateway;
        }

        public async Task<ServiceResponse<string>> GenerateToken(string userId)
        {
            ServiceResponse<string> objReturn = new ServiceResponse<string>();
            ClientTokenRequest clientTokenRequest;
            string clientToken;

            try
            {
                var gateway = this.GetGateway();
                ResourceCollection<Customer> collection = searchCustumer(userId, gateway);

                if (collection == null || collection.Count() == 0)
                {
                    clientToken = gateway.ClientToken.Generate();

                }
                else
                {
                    clientTokenRequest = new ClientTokenRequest { CustomerId = userId };
                    clientToken = gateway.ClientToken.Generate(clientTokenRequest);
                }

                objReturn = this.SetResultStatus<string>(clientToken, MessageStatus.Success, true);
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<string>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        private static ResourceCollection<Customer> searchCustumer(string UserId, IBraintreeGateway gateway)
        {
            var searchRequest = new CustomerSearchRequest().Id.Is(UserId);
            var collection = gateway.Customer.Search(searchRequest);
            return collection;
        }

        public async Task<ServiceResponse<Result<Transaction>>> SettleTransaction(string userId, decimal amount, string nonceFromTheClient, string CurrencyCode)
        {
            ServiceResponse<Result<Transaction>> objReturn = new ServiceResponse<Result<Transaction>>();
            string sResponse;
            try
            {
                var gateway = this.GetGateway();

                ResourceCollection<Customer> collection = searchCustumer(userId, gateway);

                if (collection == null || collection.Count() == 0)
                {
                    var customerRequest = new CustomerRequest
                    {
                        Id = userId,
                        PaymentMethodNonce = nonceFromTheClient,
                    };

                    Result<Customer> customerResult = gateway.Customer.Create(customerRequest);
                }
                
                var request = new TransactionRequest
                {
                    Amount = amount,
                    CurrencyIsoCode= CurrencyCode,
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    },
                    CustomerId= userId
                };

                Result<Transaction> transactionResult = gateway.Transaction.Sale(request);

                if (transactionResult.IsSuccess())
                {
                    objReturn = this.SetResultStatus<Result<Transaction>>(transactionResult, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<Result<Transaction>>(transactionResult, MessageStatus.Fail, false);
                }
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<Result<Transaction>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }
    }
}
