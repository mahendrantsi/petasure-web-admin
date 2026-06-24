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
    public interface ICategoryService
    {
        Task<ServiceResponse<CategoryViewModel>> Create(CategoryViewModel currencyViewModel);
        Task<ServiceResponse<CategoryViewModel>> Edit(CategoryViewModel currencyViewModel);
        Task<ServiceResponse<CategoryViewModel>> GetById(long id);
        Task<ServiceResponse<List<CategoryViewModel>>> GetAll(JQueryDataTableModel param);
    }
}
