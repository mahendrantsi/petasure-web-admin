using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.IService
{
    public interface IExceptionLoggerService
    {
        Task LogException(Exception exception);
        Task LogException(string exception);
    }
}
