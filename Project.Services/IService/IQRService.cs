using Project.Models.CommonModel;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IQRService
    {
        Task<ServiceResponse<QRCodeModel>> CreateQRCode(QRCodeReqModel model);
        Task<ServiceResponse<QRCodeModel>> CreateQRCodeV2(QrRequestModel model);
    }
}
