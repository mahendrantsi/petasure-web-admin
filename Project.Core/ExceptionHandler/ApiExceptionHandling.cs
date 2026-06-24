namespace Project.Core.ExceptionHandler
{
    using Project.Logger;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class ApiExceptionHandling
    {
        private readonly RequestDelegate next;
        private static ILoggerManager loggerManager;

        public static ILoggerManager LoggerManager { get => loggerManager; set => loggerManager = value; }

        // / <summary>
        // / Initializes a new instance of the <see cref="ApiExceptionHandling"/> class.
        // / </summary>
        // / <param name="next"></param>
        // / <param name="loggerManager"></param>
        public ApiExceptionHandling(RequestDelegate next, ILoggerManager loggerManager)
        {
            this.next = next;
            LoggerManager = loggerManager;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            LoggerManager.LogException(ex);
            var code = HttpStatusCode.InternalServerError; //  500 if unexpected
            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result); //  mention return type Task.
        }
    }
}