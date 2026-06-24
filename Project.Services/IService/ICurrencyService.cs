using SmartPay.Models.AdminModel;
using SmartPay.Models.CommonModel;
using SmartPay.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.IService
{
    public interface ICurrencyService
    {
        Task<ServiceResponse<CurrencyViewModel>> Create(CurrencyViewModel currencyViewModel);
        Task<ServiceResponse<CurrencyViewModel>> Edit(CurrencyViewModel currencyViewModel);
        Task<ServiceResponse<CurrencyViewModel>> GetById(long id);
        Task<ServiceResponse<List<CurrencyViewModel>>> GetAll(JQueryDataTableModel param);
    }
}
