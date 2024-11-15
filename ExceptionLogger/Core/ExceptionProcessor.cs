using ExceptionLogger.Configuration;
using ExceptionLogger.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger.Core
{
    public class ExceptionProcessor
    {
        private readonly LoggingOptions _options;
        private readonly ILogger _logger;
        private readonly string _tempDirectory;

        public ExceptionProcessor(ILogger logger, LoggingOptions options, string tempDirectory)
        {
            _logger = logger;
            _options = options;
            _tempDirectory = tempDirectory;
        }

        public void ProcessException(ExceptionContext context)
        {
            // Log the exception details
            LogExceptionDetails(context);

            // Save the exception to a temporary file
            SaveExceptionToTempFile(context);

            // Start a background task to delete old temp files
            StartTempFileCleanupTask();
        }

        private void LogExceptionDetails(ExceptionContext context)
        {
            var logBuilder = new StringBuilder();
            logBuilder.AppendLine($"Exception occurred at {context.Timestamp:yyyy-MM-dd HH:mm:ss.fff}");
            logBuilder.AppendLine($"Application: {context.ApplicationName}");
            logBuilder.AppendLine($"Environment: {context.Environment}");

            if (_options.IncludeSystemInfo)
            {
                logBuilder.AppendLine($"Machine: {context.MachineName}");
            }

            AppendExceptionDetails(logBuilder, context.Exception, 0);

            if (context.AdditionalData.Count > 0)
            {
                logBuilder.AppendLine("Additional Context:");
                foreach (var kvp in context.AdditionalData)
                {
                    logBuilder.AppendLine($"  {kvp.Key}: {kvp.Value}");
                }
            }

            _logger.LogError(context.Exception, logBuilder.ToString());
        }

        private void AppendExceptionDetails(StringBuilder builder, Exception ex, int depth)
        {
            if (ex == null || depth > _options.MaxInnerExceptionDepth) return;

            builder.AppendLine($"Exception Type: {ex.GetType().FullName}");
            builder.AppendLine($"Message: {ex.Message}");

            if (_options.IncludeStackTrace && !string.IsNullOrEmpty(ex.StackTrace))
            {
                builder.AppendLine("Stack Trace:");
                builder.AppendLine(ex.StackTrace);
            }

            if (_options.IncludeInnerExceptions && ex.InnerException != null)
            {
                builder.AppendLine("Inner Exception:");
                AppendExceptionDetails(builder, ex.InnerException, depth + 1);
            }
        }

        private void SaveExceptionToTempFile(ExceptionContext context)
        {
            var fileName = $"exception_{context.Timestamp:yyyyMMddHHmmss}.txt";
            var filePath = Path.Combine(_tempDirectory, fileName);

            try
            {
                File.WriteAllText(filePath, $@"
                    Exception Details:
                    Application Name: {context.ApplicationName}
                    Environment: {context.Environment}
                    Machine Name: {context.MachineName}
                    Request ID: {context.RequestId}
                    Timestamp: {context.Timestamp}

                    Additional Data:
                    {context.AdditionalData}

                    Exception Type: {context.Exception.GetType().Name}
                    Message: {context.Exception.Message}
                    Stack Trace:
                    {context.Exception.StackTrace}
                ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save exception to temporary file: {FilePath}", filePath);
            }
        }

        private void StartTempFileCleanupTask()
        {
            // Run a background task to delete old temp files every 24 hours
            Task.Run(async () =>
            {
                while (true)
                {
                    await DeleteOldTempFiles();
                    await Task.Delay(TimeSpan.FromHours(24));
                }
            });
        }

        private async Task DeleteOldTempFiles()
        {
            try
            {
                var now = DateTime.UtcNow;
                var oldestAllowedTime = now.AddDays(-1);

                foreach (var file in Directory.GetFiles(_tempDirectory))
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTimeUtc < oldestAllowedTime)
                    {
                        File.Delete(file);
                        _logger.LogDebug("Deleted old temporary exception file: {FilePath}", file);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to clean up temporary exception files");
            }
        }
    }
}
