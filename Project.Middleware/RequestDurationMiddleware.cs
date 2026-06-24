using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
namespace Project.Middleware
{
     // / <summary>
    // / Middleware to time the request duration.
    // / Should be placed first in the middleware pipeline to enable accurate timing.
    // / </summary>
    public class RequestDurationMiddleware
    {
        private const string RequestDurationHeader = "X-RequestTime-ms";

        private readonly RequestDelegate next;

        public RequestDurationMiddleware(RequestDelegate next) => this.next = next;

        public Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            //  tuck in just before response headers are added to response (they cannot be updated once sent)
            context.Response.OnStarting(() =>
            {
                watch.Stop();

                var timeForCompletedRequest = watch.ElapsedMilliseconds;
                context.Response.Headers[RequestDurationHeader] = timeForCompletedRequest.ToString();

                return Task.CompletedTask;
            });

            return this.next(context);
        }
    }
}
