using Microsoft.AspNetCore.Mvc;
using System.Net;
using WeatherService.Api.Business.Services;

namespace WeatherService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public partial class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherApiService _weatherApiService;

        public WeatherController(ILogger<WeatherController> logger, IWeatherApiService weatherApiService)
        {
            _logger = logger;
            _weatherApiService = weatherApiService;
        }

        [HttpGet("{city}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRealTimeWeather(string city)
        {
            var result = await _weatherApiService.GetWeatherInformationAsync(city);

            if (result.Key != HttpStatusCode.OK)
            {
                _logger.LogError(message: result.Value);
            }

            return result.Key switch
            {
                HttpStatusCode.OK => Ok(result.Value),
                _ => BadRequest(result.Value)
            };
        }
    }
}