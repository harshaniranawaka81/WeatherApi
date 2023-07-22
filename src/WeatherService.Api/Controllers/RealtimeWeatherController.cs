using Microsoft.AspNetCore.Mvc;
using System.Net;
using WeatherService.Api.Business.Services;

namespace WeatherService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class RealtimeWeatherController : ControllerBase
    {
        private readonly ILogger<RealtimeWeatherController> logger;
        private readonly IWeatherApiService weatherApiService;

        public RealtimeWeatherController(ILogger<RealtimeWeatherController> logger, IWeatherApiService weatherApiService)
        {
            this.logger = logger;
            this.weatherApiService = weatherApiService;
        }

        [HttpGet("{city}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRealTimeWeather(string city)
        {
            var result = await weatherApiService.GetRealTimeWeatherAsync(city);

            if (result.Key == HttpStatusCode.NotFound)
            {
                return NotFound(result.Value);
            }

            if (result.Key == HttpStatusCode.BadRequest)
            {
                return BadRequest(result.Value);
            }

            return Ok(result.Value);
        }

        [HttpGet("{city}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetRealTimeWeatherWithAstronomy(string city)
        {
            var result = await weatherApiService.GetRealTimeWeatherAsync(city);

            if (result.Key == HttpStatusCode.NotFound)
            {
                return NotFound(result.Value);
            }

            if (result.Key == HttpStatusCode.BadRequest)
            {
                return BadRequest(result.Value);
            }

            return Ok(result);
        }
    }
}