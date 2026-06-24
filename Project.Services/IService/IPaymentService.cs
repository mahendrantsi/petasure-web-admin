using Braintree;
using SmartPay.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.IService
{
    public interface IPaymentService
    {
        Task<ServiceResponse<string>> GenerateToken(string UserId);

        Task<ServiceResponse<Result<Transaction>>> SettleTransaction(string userId, decimal amount, string nonceFromTheClient, string CurrencyCode);
    }
}
