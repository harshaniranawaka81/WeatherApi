using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WeatherService.Api.Business.Clients;
using WeatherService.Api.Business.Constants;

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

        //public virtual async Task<InlineResponse200> GetRealTimeWeatherAsync(string city)
        //{
        //    var apiKey = config[$"{Constants.Constants.WEATHERAPI_SECTION}:{Constants.Constants.APIKEY}"];

        //    if (string.IsNullOrEmpty(apiKey))
        //    {
        //        const string message = $"{Constants.Constants.WEATHER_API_KEY} has not set!";
        //        throw new ArgumentNullException(message);
        //        logger.LogError(message);
        //    }

        //    Configuration.Default.ApiKey.Add("key", apiKey);
        //    var apiInstance = new APIsApi();
        //    var result = await apiInstance.RealtimeWeatherAsync(city);
        //    return result;
        //}

        public virtual async Task<HttpResponseMessage> GetRealTimeWeatherAsync(string city)
        {
            var url = GetUrl(Constants.Constants.REALTIME_WEATHER_ENDPOINT);
            var response = await _weatherApiClient.GetAsync(url);

            return response;
        }

        private string GetUrl(string type)
        {
            var baseUrl = _config[$"{Constants.Constants.WEATHERAPI_SECTION}:{Constants.Constants.BASE_URL}"];

            var message = string.Empty;

            if (string.IsNullOrEmpty(baseUrl))
            {
                message = "Base Url has not been set in the configuration!";
            }

            var endpoint = _config[$"{Constants.Constants.WEATHERAPI_SECTION}:{type}"];

            if (string.IsNullOrEmpty(endpoint))
            {
                message = $"{type} has not been set in the configuration!";
            }

            if (!string.IsNullOrEmpty(message))
            {
                _logger.LogError(message);
                throw new ArgumentNullException(message);
            }

            return $"{baseUrl}{endpoint}";
        }
    }
}
