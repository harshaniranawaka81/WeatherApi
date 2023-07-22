using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WeatherService.Api.Business.Clients
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<WeatherApiClient> _logger;

        public WeatherApiClient(IHttpClientFactory httpClientFactory, ILogger<WeatherApiClient> logger, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _config = config;
        }

        public async Task<HttpResponseMessage> SendAsync(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = _httpClientFactory.CreateClient();

            var apiKey = _config[$"{Constants.Constants.WEATHERAPI_SECTION}:{Constants.Constants.APIKEY}"];

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("key", apiKey);

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            return httpResponseMessage;
        }

    }

}
