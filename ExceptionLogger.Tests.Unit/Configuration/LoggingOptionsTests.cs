using ExceptionLogger.Configuration;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger.Tests.Unit.Configuration
{
    public class LoggingOptionsTests
    {
        [Fact]
        public void LoggingOptions_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var options = new LoggingOptions();

            // Assert
            options.IncludeStackTrace.Should().BeTrue();
            options.IncludeInnerExceptions.Should().BeTrue();
            options.IncludeSystemInfo.Should().BeTrue();
            options.MaxInnerExceptionDepth.Should().Be(5);
        }
    }
}
