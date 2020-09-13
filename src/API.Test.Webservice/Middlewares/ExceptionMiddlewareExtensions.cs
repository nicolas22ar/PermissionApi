using System;
using API.Test.Webservice.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace API.Test.WebService.Middlewares
{
	/// <summary>
	/// Static class for middleware exceptions
	/// </summary>
	public static class ExceptionMiddlewareExtensionsa
	{
        /// <summary>
        /// Handles any exception and produces the corresponding log, status code and response message
        /// </summary>
        /// <param name="builder">The Microsoft.AspNetCore.Builder.IApplicationBuilder</param>
        /// <param name="logger">logger</param>
        /// <returns>The Microsoft.AspNetCore.Builder.IApplicationBuilder</returns>
        public static IApplicationBuilder UseErrorHandling(
            this IApplicationBuilder builder,
            ILogger logger)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>(logger);
        }
    }
}