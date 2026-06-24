namespace Project.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Project.Models;
    using Project.Models.APIModel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json.Linq;
    using Project.Core.Model;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using static Project.Core.Extension.APICoreRes;
    using Project.Core.Extension; 
    using Project.Services.IService;



    //  TODO: parse jsonoptions (date) and header name(s) as params

    // / <summary>
    // / Wraps all responses in a common json response.
    // / </summary>
    public class CommonResponseMiddleware : DisposableBase, IDisposable
    {
        private const string ApiVersionHeader = "X-ApiVersion";

        private readonly RequestDelegate next;
        private readonly ApiMiddlewareOptions options;
        private readonly ILogger<CommonResponseMiddleware> logger;
        private readonly JsonSerializerOptions jsonSerializerOptions;
         

        // / <summary>
        // / Initializes a new instance of the <see cref="CommonResponseMiddleware"/> class.
        // / </summary>
        // / <param name="options">The options.</param>
        // / <param name="next">The next.</param>
        // / <param name="logger">The logger.</param>
        // / <param name="mvcJsonOptions">The MVC json options.</param>
        public CommonResponseMiddleware(
            ApiMiddlewareOptions options,
            RequestDelegate next,
            ILogger<CommonResponseMiddleware> logger,
            IOptions<JsonSerializerOptions> jsonSerialiserOptions
            )
        {
            this.next = next;
            this.logger = logger;
            this.options = options;
            this.jsonSerializerOptions = jsonSerialiserOptions.Value;
        }

        // / <summary>
        // / Asynchronous method invoked in the middleware pipeline
        // / </summary>
        // / <param name="context">The context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var existingBody = context.Response.Body;

            using (var newBody = new MemoryStream())
            {
                context.Response.Body = newBody;

                try
                {
                    await this.next(context);

                    context.Response.Body = existingBody;
                    context.Response.ContentType = "application/json";

                    var result = await FormatResponse(newBody);

                    await this.BuildApiResponse(context, result);
                }
                catch (Exception ex)
                {
                    context.Response.Body = existingBody;
                    await this.HandleException(context, ex);
                }
            }
        }

        private async Task BuildApiResponse(HttpContext context, string result)
        {
            var statusCode = context.Response.StatusCode;

            if (statusCode >= 200 && statusCode <= 299)
            {
                await this.HandleSuccess(context, result);
            }
            else if (statusCode == 400)
            {
                await this.HandleBadRequestResponse(context, result);
            }
            else
            {
                await this.HandleNonSuccess(context, result);
            }
        }

        private async Task HandleBadRequestResponse(HttpContext context, string result)
        {
            IEnumerable<ApiError> errors;
            object error = new object();
            try
            {
                BadRequestError response = JsonSerializer.Deserialize<BadRequestError>(result);

                if (response.title is null)
                {
                    throw new Exception();
                }

                // if (!result.Contains("FieldError"))
                // {
                //     response.FieldError = JsonSerializer.Deserialize<dynamic>(result);
                // }

                   errors = response.error == null
                    ? response.errors?.SelectMany(e => e.Value
                        .Select(v => new ApiError
                        {
                            Message = v,
                            Field = e.Key,
                        }))
                    : new List<ApiError>
                    {
                    new ApiError
                        {
                            Message = response.error?.message,
                            Field = response.error?.code,
                        },
                    };

            }
            catch
            {
                
               var response = JsonSerializer.Deserialize<ServiceResponse>(result);

            
                    errors = new List<ApiError>
                    {
                       new ApiError
                        {
                            Message = response.message,
                            Field = "ErrorMessage",
                            Data = response.data
                        },
                    };

                //var response = JsonSerializer.Deserialize<ResponseData>(result);

                //errors = new List<ApiError>
                //    {
                //    new ApiError
                //        {
                //            Message = response.coreRes.userMessage,
                //            Field = nameof (response.coreRes.userMessage),
                //        },
                //};

            }
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse
            {
                //Error= ValidJsonObject(result),
                Errors = errors?? new List<ApiError>(), 
                HasErrors = true,
                Path = context.Request.Path,
                StatusCode = context.Response.StatusCode,
                Version = context.Request.Headers[ApiVersionHeader].ToString(),
            }));
        }

        private async Task HandleSuccess(HttpContext context, string result)
        {
            var apiResponse = new ApiResponse
            {
                Path = context.Request.Path,
                StatusCode = context.Response.StatusCode,
                Result = ValidJsonObject(result),
                HasErrors = false,
                Version = context.Request.Headers[ApiVersionHeader].ToString()
            };

            var serialisedResponse = JsonSerializer.Serialize(apiResponse, this.jsonSerializerOptions);
            context.Response.ContentLength = Encoding.UTF8.GetByteCount(serialisedResponse);

            await context.Response.WriteAsync(serialisedResponse);
        }

        private async Task HandleNonSuccess(HttpContext context, string result)
        {
            var apiResponse = new ApiResponse
            {
                Errors = new List<ApiError>
                {
                    new ApiError { Message = ValidJsonObject(result) }
                },
                Path = context.Request.Path,
                StatusCode = context.Response.StatusCode,
                HasErrors = true,
                Version = context.Request.Headers[ApiVersionHeader].ToString()
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse, this.jsonSerializerOptions));
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var apiResponse = new ApiResponse
            {
                Errors = new List<ApiError> {
                    new ApiError { Message = this.options.GenericMessage ?? "An error has occurred.  Please try again in a few minutes.  If the problem persists, please talk to someone." }
                },
                HasErrors = true,
                Id = Guid.NewGuid().ToString(),
                Path = context.Request.Path,
                StatusCode = StatusCodes.Status500InternalServerError,
                Version = context.Request.Headers["X-ApiVersion"].ToString()
            };

            var innerExceptionMessage = GetInnermostExceptionMessage(exception);
            //  TODO: determine error level (critical vs error)
            //  TODO: **** ErrorId here causes an "System.IndexOutOfRangeException: Index was outside the bounds of the array." for (eg) Plaid.ApiError - WHYYYYYYY??
            try
            {
                this.logger.Log(LogLevel.Error, exception, $"BADNESS!!! {innerExceptionMessage} -- {{ErrorId}}.", apiResponse.Id);
            }
            catch
            {
                this.logger.Log(LogLevel.Error, exception, $"BADNESS!!! {innerExceptionMessage}");
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse, this.jsonSerializerOptions));
        }

        private static object ValidJsonObject(string json)
        {
            json = json.Trim();

            if (!(json.StartsWith("{") && json.EndsWith("}")) &&
                !(json.StartsWith("[") && json.EndsWith("]")))
            {
                return json;
            }

            return JsonSerializer.Deserialize<object>(json);
        }

        private static async Task<string> FormatResponse(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using (var sr = new StreamReader(stream))
            {
                var data = await sr.ReadToEndAsync();
                return data;
            }
        }

        [Pure]
        private static string GetInnermostExceptionMessage(Exception exception)
        {
            return exception.InnerException != null
                ? GetInnermostExceptionMessage(exception.InnerException)
                : exception.Message;
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }

    public class ServiceResponse
    { 

        public string message { get; set; }
        public string code{ get; set; }
        public string status { get; set; }


        public DateTime StartOn { get; set; }

        public DateTime EndOn { get; set; }

        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public dynamic data{get;set;}

        public ServiceResponse()
        {
            this.StartOn = DateTime.UtcNow;
        }
    }
}
