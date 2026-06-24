using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project.Data.DBEntities;
using Project.Persistence.UOW;
using Project.Services.IService;
using Project.Services.Service;
using System;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Project.Web.WebResource.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)//, IExceptionLoggerService exceptionLoggerService
        {
            this.next = next;
            _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>(); 
            //_exceptionLogger = exceptionLoggerService;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private  Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            try
            {
                using (var db = new ProjectDbContext())
                {
                    db.Add<ExceptionLogger>(new ExceptionLogger() { InnerException = ex.InnerException?.ToString(), Exception = ex.ToString() });
                    db.SaveChanges();
                }

            }
            catch 
            {

            }

            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var errorCode = Guid.NewGuid().ToString();

            if (ex is AuthenticationException)
            {
                code = HttpStatusCode.Unauthorized;
                context.Response.Redirect("/Account/Login");
            }
            //var result = JsonConvert.SerializeObject(new
            //{
            //    Status = "Error",
            //    ErrorCode = (int)code,
            //    ErrorMessage = ex.Message,
            //});
            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)code;
            //return context.Response.WriteAsync(result);

            if (ex.Message == "CustomError")
            {
                errorCode = ex.InnerException.Message;
            }
            
            context.Response.Redirect($"/Home/Error/{errorCode}");
            return Task.FromResult(new { });
        }
    }
}
