using ExceptionLogger.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger.Tests.Unit.Models
{
    public class ExceptionContextTests
    {
        [Fact]
        public void ExceptionContext_WhenCreated_ShouldHaveCorrectDefaults()
        {
            // Arrange & Act
            var context = new ExceptionContext();

            // Assert
            context.AdditionalData.Should().NotBeNull();
            context.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            context.MachineName.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ExceptionContext_WithException_ShouldStoreCorrectly()
        {
            // Arrange
            var exception = new Exception("Test exception");

            // Act
            var context = new ExceptionContext
            {
                Exception = exception,
                ApplicationName = "TestApp",
                Environment = "Test"
            };

            // Assert
            context.Exception.Should().BeSameAs(exception);
            context.ApplicationName.Should().Be("TestApp");
            context.Environment.Should().Be("Test");
        }
    }
}
