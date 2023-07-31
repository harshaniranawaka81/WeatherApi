using Microsoft.AspNetCore.Mvc;
using System.Net;
using WeatherService.Api.Business.Factories;
using WeatherService.Api.Business.Services;
using WeatherService.Api.Business.Strategies;

namespace WeatherService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherStrategyFactory _weatherStrategyFactory;

        public WeatherController(ILogger<WeatherController> logger, IWeatherStrategyFactory weatherStrategyFactory)
        {
            _logger = logger;
            _weatherStrategyFactory = weatherStrategyFactory;
        }

        [HttpGet("{city}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRealTimeWeather(string city)
        {
            var weatherStrategy = _weatherStrategyFactory.Create(1);

            var result = await weatherStrategy.GetWeatherInformationAsync(city);

            if (result.Key != HttpStatusCode.OK && result.Key != HttpStatusCode.NotFound)
            {
                _logger.LogError(message: result.Value);
            }

            return result.Key switch
            {
                HttpStatusCode.OK => Ok(result.Value),
                HttpStatusCode.NotFound => NotFound(result.Value),
                _ => BadRequest(result.Value)
            };
        }

        [HttpGet("{city}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetRealTimeWeatherWithAstronomy(string city)
        {
            var weatherStrategy = _weatherStrategyFactory.Create(2);

            var result = await weatherStrategy.GetWeatherInformationAsync(city);

            if (result.Key != HttpStatusCode.OK && result.Key != HttpStatusCode.NotFound)
            {
                _logger.LogError(message: result.Value);
            }

            return result.Key switch
            {
                HttpStatusCode.OK => Ok(result.Value),
                HttpStatusCode.NotFound => NotFound(result.Value),
                _ => BadRequest(result.Value)
            };
        }
    }
}