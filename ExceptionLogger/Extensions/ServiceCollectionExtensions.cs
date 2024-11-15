using ExceptionLogger.Configuration;
using ExceptionLogger.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExceptionLogging(
            this IServiceCollection services,
            Action<LoggingOptions> configure = null)
        {
            var options = new LoggingOptions();
            configure?.Invoke(options);

            // Create the temp directory if it doesn't exist
            var tempDirectory = Path.Combine(Path.GetTempPath(), "ExceptionLogger");
            Directory.CreateDirectory(tempDirectory);

            services.AddSingleton(options);
            services.AddSingleton<ExceptionProcessor>(sp =>
                new ExceptionProcessor(
                    sp.GetRequiredService<ILogger<ExceptionProcessor>>(),
                    options,
                    tempDirectory));

            return services;
        }
    }
}
