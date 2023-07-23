using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherService.Api.Business.Clients;
using WeatherService.Api.Business.Constants;
using WeatherService.Api.Business.DTO;

namespace WeatherService.Api.Business.Services
{
    public class WeatherApiService : IWeatherApiService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<WeatherApiService> _logger;
        private readonly IWeatherApiClient _weatherApiClient;

        public WeatherApiService(IConfiguration config, ILogger<WeatherApiService> logger, IWeatherApiClient weatherApiClient)
        {
            _config = config;
            _logger = logger;
            _weatherApiClient = weatherApiClient;
        }

        public virtual async Task<KeyValuePair<HttpStatusCode, string>> GetRealTimeWeatherAsync(string city)
        {
            ValidateInputs(city, Constants.Constants.REALTIME_WEATHER_ENDPOINT, out string? url);

            url = $"{url}?q={city}";

            var result = await _weatherApiClient.GetAsync(url);

            if (result.Key != HttpStatusCode.OK) return result;

            dynamic json = JObject.Parse(result.Value);
            var realtimeWeatherObj = new RealtimeWeather()
            {
                City = json.location.name,
                Region = json.location.region,
                Country = json.location.country,
                LocalTime = json.location.localtime,
                Temperature = json.current.temp_c,
            };

            return new KeyValuePair<HttpStatusCode, string>(HttpStatusCode.OK, JsonConvert.SerializeObject(realtimeWeatherObj));

        }

        private void ValidateInputs(string city, string type,out string? url)
        {
            string? message;

            if (string.IsNullOrEmpty(city))
            {
                message = "Parameter city has not been passed!";
                _logger.LogError(message);
                throw new ValidationException(message);
            }

            var baseUrl = _config[$"{Constants.Constants.WEATHERAPI_SECTION}:{Constants.Constants.BASE_URL}"];

            if (string.IsNullOrEmpty(baseUrl))
            {
                message = "Base Url has not been set in the configuration!";
                _logger.LogError(message);
                throw new ValidationException(message);
            }

            var endpoint = _config[$"{Constants.Constants.WEATHERAPI_SECTION}:{type}"];

            if (string.IsNullOrEmpty(endpoint))
            {
                message = $"{type} has not been set in the configuration!";
                _logger.LogError(message);
                throw new ValidationException(message);
            }

            url = $"{baseUrl}{endpoint}";
        }
    }
}
