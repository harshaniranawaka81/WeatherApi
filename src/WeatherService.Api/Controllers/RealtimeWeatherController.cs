using Microsoft.AspNetCore.Mvc;
using System.Net;
using WeatherService.Api.Business.Services;

namespace WeatherService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public partial class RealtimeWeatherController : ControllerBase
    {
        private readonly ILogger<RealtimeWeatherController> _logger;
        private readonly IWeatherApiService _weatherApiService;

        public RealtimeWeatherController(ILogger<RealtimeWeatherController> logger, IWeatherApiService weatherApiService)
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
            var result = await _weatherApiService.GetRealTimeWeatherAsync(city);

            return result.Key switch
            {
                HttpStatusCode.NotFound => NotFound(result.Value),
                HttpStatusCode.BadRequest => BadRequest(result.Value),
                _ => Ok(result.Value)
            };
        }
    }
}