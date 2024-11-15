using ExceptionLogger.Core;
using ExceptionLogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionProcessor _processor;
        private readonly string _applicationName;
        private readonly string _environment;

        public ExceptionLoggingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionLoggingMiddleware> logger,
            Configuration.LoggingOptions options,
            string applicationName,
            string environment)
        {
            _next = next;
            _processor = new ExceptionProcessor(logger, options, options.PathToStore);
            _applicationName = applicationName;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var exceptionContext = new ExceptionContext
                {
                    Exception = ex,
                    ApplicationName = _applicationName,
                    Environment = _environment,
                    RequestId = context.TraceIdentifier,
                    UserId = context.User?.Identity?.Name
                };

                // Add request-specific data
                exceptionContext.AdditionalData["Path"] = context.Request.Path;
                exceptionContext.AdditionalData["Method"] = context.Request.Method;
                exceptionContext.AdditionalData["QueryString"] = context.Request.QueryString.ToString();

                _processor.ProcessException(exceptionContext);
                throw; // Re-throw to maintain the original exception flow
            }
        }
    }
}
