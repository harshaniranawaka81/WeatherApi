using Microsoft.AspNetCore.Mvc;
using System.Net;
using WeatherService.Api.Business.DTO;
using WeatherService.Api.Business.Factories;
using WeatherService.Api.Business.Services;
using WeatherService.Api.Business.Strategies;

namespace WeatherService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherStrategyFactory _weatherStrategyFactory;

        public WeatherController(ILogger<WeatherController> logger, IWeatherStrategyFactory weatherStrategyFactory)
        {
            _logger = logger;
            _weatherStrategyFactory = weatherStrategyFactory;
        }

        /// <summary>
        /// Gets the real time weather.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/Weather/{city}?api-version=1
        ///
        /// </remarks>
        /// <returns>IEnumerable of slugs</returns>
        /// <response code="200">If a result is found</response>
        /// <response code="404">If a result is not found</response>
        /// <response code="400">If there is an error</response>
        [HttpGet("{city}")]
        [ProducesResponseType(typeof(RealtimeWeather), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Gets the real time weather with astronomy.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/Weather/{city}?api-version=2
        ///
        /// </remarks>
        /// <returns>IEnumerable of slugs</returns>
        /// <response code="200">If a result is found</response>
        /// <response code="404">If a result is not found</response>
        /// <response code="400">If there is an error</response>
        [HttpGet("{city}")]
        [ProducesResponseType(typeof(RealtimeWeatherWithAstronomy), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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