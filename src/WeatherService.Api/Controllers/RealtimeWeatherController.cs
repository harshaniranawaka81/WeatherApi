using Microsoft.AspNetCore.Mvc;
using WeatherService.Api.Business.Services;

namespace WeatherService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RealtimeWeatherController : ControllerBase
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
        public async Task<IActionResult> GetRealTimeWeather(string city)
        {
            var result = await weatherApiService.GetRealTimeWeatherAsync(city);

            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }
    }
}