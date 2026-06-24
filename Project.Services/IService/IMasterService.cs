using SmartPay.Models.Master;
using SmartPay.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.IService
{
    public interface IMasterService
    {
        Task<ServiceResponse<List<CategoryDTO>>> getCategories();
        Task<ServiceResponse<List<CurrencyDTO>>> getCurrencies();
    }
}
