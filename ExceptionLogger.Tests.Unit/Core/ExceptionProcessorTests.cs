using Castle.Core.Logging;
using ExceptionLogger.Configuration;
using ExceptionLogger.Core;
using ExceptionLogger.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionLogger.Tests.Unit.Core
{
    public class ExceptionProcessorTests
    {
        private readonly Mock<Microsoft.Extensions.Logging.ILogger> _logger;
        private readonly LoggingOptions _options;
        private readonly ExceptionProcessor _processor;
        public ExceptionProcessorTests()
        {
            // Setup for each test
            _logger = new Mock<Microsoft.Extensions.Logging.ILogger>();
            _options = new LoggingOptions
            {
                IncludeStackTrace = true,
                IncludeInnerExceptions = true,
                MaxInnerExceptionDepth = 3
            };
            _processor = new ExceptionProcessor(_logger.Object, _options, _options.PathToStore);
        }

        [Fact]
        public void ProcessException_WithBasicException_ShouldLogCorrectly()
        {
            // Arrange
            var testException = new Exception("Test exception message");
            var context = new ExceptionContext
            {
                Exception = testException,
                ApplicationName = "TestApp",
                Environment = "Test"
            };

            // Act
            _processor.ProcessException(context);

            // Assert
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString().Contains("Test exception message") &&
                        v.ToString().Contains("TestApp")),
                    testException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void ProcessException_WithInnerException_ShouldLogBothExceptions()
        {
            // Arrange
            var innerException = new InvalidOperationException("Inner exception message");
            var outerException = new Exception("Outer exception message", innerException);
            var context = new ExceptionContext
            {
                Exception = outerException,
                ApplicationName = "TestApp",
                Environment = "Test"
            };

            // Act
            _processor.ProcessException(context);

            // Assert
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString().Contains("Outer exception message") &&
                        v.ToString().Contains("Inner exception message")),
                    outerException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void ProcessException_WithAdditionalData_ShouldIncludeInLog()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var context = new ExceptionContext
            {
                Exception = exception,
                ApplicationName = "TestApp",
                Environment = "Test",
                AdditionalData = new Dictionary<string, object>
                {
                    ["UserId"] = "12345",
                    ["RequestPath"] = "/api/test"
                }
            };

            // Act
            _processor.ProcessException(context);

            // Assert
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString().Contains("UserId") &&
                        v.ToString().Contains("12345") &&
                        v.ToString().Contains("RequestPath") &&
                        v.ToString().Contains("/api/test")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void ProcessException_WithoutStackTrace_ShouldNotIncludeStackTrace()
        {
            // Arrange
            _options.IncludeStackTrace = false;
            var exception = new Exception("Test exception");
            var context = new ExceptionContext
            {
                Exception = exception,
                ApplicationName = "TestApp",
                Environment = "Test"
            };

            // Act
            _processor.ProcessException(context);

            // Assert
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        !v.ToString().Contains("Stack Trace")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
