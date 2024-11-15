using ExceptionLogger.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionLogging(
            this IApplicationBuilder app,
            string applicationName,
            string environment)
        {
            return app.UseMiddleware<ExceptionLoggingMiddleware>(
                applicationName,
                environment);
        }
    }
}
