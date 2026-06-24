using Project.Data.DBEntities;
using Project.Persistence.UOW;
using Project.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Service
{
    public class ExceptionLoggerService: IExceptionLoggerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExceptionLoggerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task LogException(Exception exception)
        {
            using (ProjectDbContext ProjectDbContext = new ProjectDbContext())
            {
                ProjectDbContext.ExceptionLogger.Add(new ExceptionLogger() { InnerException = exception.InnerException?.ToString(), Exception = exception.ToString() });
                ProjectDbContext.SaveChanges();
            }
        }

        public async Task LogException(string exception)
        {
            using (ProjectDbContext ProjectDbContext = new ProjectDbContext())
            {
                ProjectDbContext.ExceptionLogger.Add(new ExceptionLogger() { InnerException = "", Exception = exception });
                ProjectDbContext.SaveChanges();
            }
        }
    }
}
