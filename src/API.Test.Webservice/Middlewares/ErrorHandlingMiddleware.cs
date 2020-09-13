using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Serilog;
using API.Test.Infrastructure.Concrete.Exceptions;

namespace API.Test.Webservice.Middlewares
{
    /// <summary>
    /// ErrorHandlingMiddleware
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// ErrorHandlingMiddleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private string BuildExceptionMessage(Exception exc)
        {
            var message = $"\r\n\t{exc.GetType()}: {exc.Message}";

            if (exc.InnerException != null)
            {
                message += BuildExceptionMessage(exc.InnerException);
            }
            return message;
        }

        private string BuildMessage(Exception exc, HttpStatusCode code, string referer = null)
        {
            var message = BuildExceptionMessage(exc);

            if (!string.IsNullOrEmpty(referer))
            {
                message += $"\r\n\tReferer: {referer}\r\n\tResponse: Status {(int)code} ({code})";
            }
            return message;
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = exception.Message;

            switch (exception)
            {
                case UnauthorizedAccessException _:
                    {
                        code = HttpStatusCode.Forbidden;
                        _logger.Information(BuildMessage(exception, code,
                            context.Request.Headers["Referer"].ToString()));
                        break;
                    }
                case EntityNotFoundException _:
                    {
                        code = HttpStatusCode.NotFound;
                        _logger.Debug(BuildMessage(exception, code));
                        break;
                    }
                case DuplicatedEntityException _:
                    {
                        code = HttpStatusCode.Conflict;
                        _logger.Debug(BuildMessage(exception, code));
                        break;
                    }

                default:
                    {
                        message = "Internal server error";
                        _logger.Error(BuildMessage(exception, code));
                        break;
                    }
            }

            var result = JsonConvert.SerializeObject(new { error = message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
