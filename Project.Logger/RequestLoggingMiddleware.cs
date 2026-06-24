namespace Project.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.Extensions.Logging;

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public string GetRequestInfo(HttpContext context)
        {
            context.Request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            context.Request.Body.Position = 0;
            var requestBody = Encoding.UTF8.GetString(buffer);
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            var builder = new StringBuilder(Environment.NewLine);
            foreach (var header in context.Request.Headers)
            {
                builder.AppendLine($"{header.Key}:{header.Value}");
            }

            string url = context.Request.GetDisplayUrl().Trim();
            builder.AppendLine($"Url:{url}");
            string getEncodedUrl = context.Request.GetEncodedUrl().Trim();
            builder.AppendLine($"GetEncodedUrl:{getEncodedUrl}");
            string queryString = context.Request.QueryString.ToString().Trim();
            builder.AppendLine($"QueryString:{queryString}");

            builder.AppendLine($"Request body:{requestBody}");
            return builder.ToString();
        }

        public async Task Invoke(HttpContext context)
        {
            string msg = this.GetRequestInfo(context);
            new LoggerManager().LogTrace(msg);
            await this.next(context);
        }
    }
}