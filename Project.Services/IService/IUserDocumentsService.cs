using Microsoft.AspNetCore.Http;
using Project.Core.Enum;
using Project.Data.DBEntities;
using Project.Models.ProfileModel;
using Project.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IUserDocumentsService
    {
        Task<ServiceResponse<string>> SaveFileList(List<DocSaveModel> docs);
    }
}
