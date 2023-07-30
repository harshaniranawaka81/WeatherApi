using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace WeatherService.Api.Business.Clients
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public WeatherApiClient(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<KeyValuePair<HttpStatusCode, string>> GetAsync(string url)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var apiKey = _config[$"{Constants.Constants.WEATHERAPI_SECTION}:{Constants.Constants.APIKEY}"];

            httpClient.DefaultRequestHeaders.Add("key", apiKey);

            var httpResponseMessage = await httpClient.GetAsync(url);

            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            return new KeyValuePair<HttpStatusCode, string>(httpResponseMessage.StatusCode, content);
        }

    }

}
