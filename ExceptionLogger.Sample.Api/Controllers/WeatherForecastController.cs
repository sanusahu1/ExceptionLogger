// Controllers/WeatherForecastController.cs
using Microsoft.AspNetCore.Mvc;

namespace ExceptionLogger.Sample.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild",
            "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("error")]
        public IActionResult GetError()
        {
            throw new Exception("This is a test exception");
        }

        [HttpGet("nested-error")]
        public IActionResult GetNestedError()
        {
            try
            {
                throw new InvalidOperationException("Inner exception");
            }
            catch (Exception ex)
            {
                throw new Exception("Outer exception", ex);
            }
        }

        [HttpGet("custom-error")]
        public IActionResult GetCustomError()
        {
            try
            {
                throw new Exception("Custom exception with context");
            }
            catch (Exception ex)
            {
                HttpContext.Items["CustomData"] = new Dictionary<string, object>
                {
                    ["UserId"] = "12345",
                    ["RequestPath"] = HttpContext.Request.Path,
                    ["CustomValue"] = "Test Value"
                };
                throw;
            }
        }
    }
}