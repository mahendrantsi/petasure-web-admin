using SmartPay.Data.DBEntities;
using SmartPay.Models.CommonModel;
using SmartPay.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.IService
{
    public interface IHelpService
    {
        Task<ServiceResponse<String>> SaveUserMessage(EnquiryViewModel dto);
        Task<ServiceResponse<List<EnquiryListResult>>> GetEnquires(JQueryDataTableModel param);
        Task<ServiceResponse<ContentMaster>> getContent(string ContentType);
    }
}
