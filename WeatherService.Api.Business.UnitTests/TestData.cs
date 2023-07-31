using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq.Protected;
using Moq;
using Newtonsoft.Json.Linq;

namespace WeatherService.Api.Business.UnitTests
{
    public static class TestData
    {
        public static IConfiguration CreateConfigMock()
        {
            var appSettings = new Dictionary<string, string> {
                {"WeatherApi:BaseUrl", "http://api.weatherapi.com/v1"},
                {"WeatherApi:ApiKey", "e37c5b71bde441bcbd3192338231807"},
                {"WeatherApi:RealtimeWeatherEndpoint", "/current.json"},
                {"WeatherApi:AstronomyEndpoint", "/astronomy.json"},
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettings)
                .Build();

            return configuration;
        }

        public static HttpClient CreateHttpClientMock(HttpResponseMessage httpResponseMessage)
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);


            // create the HttpClient
            return new HttpClient(httpMessageHandlerMock.Object);
        }

        public static string PrepareOkJsonObject_GetRealTimeWeather()
        {
            const string json = "{\r\n  \"location\": {\r\n    \"name\": \"London\",\r\n    \"region\": \"City of London, Greater London\",\r\n    \"country\": \"United Kingdom\",\r\n    \"lat\": 51.52,\r\n    \"lon\": -0.11,\r\n    \"tz_id\": \"Europe/London\",\r\n    \"localtime_epoch\": 1690135710,\r\n    \"localtime\": \"2023-07-23 19:08\"\r\n  },\r\n  \"current\": {\r\n    \"temp_c\": 22,\r\n    \"temp_f\": 71.6,\r\n    \"condition\": {},\r\n    \"uv\": 6\r\n  }\r\n}";

            var token = JToken.Parse(json);

            return token.ToString();
        }

        public static string PrepareOkJsonObject_GetRealTimeWeatherWithAstronomy()
        {
            const string json = "{\r\n  \"location\": {\r\n    \"name\": \"London\",\r\n    \"region\": \"City of London, Greater London\",\r\n    \"country\": \"United Kingdom\",\r\n    \"lat\": 51.52,\r\n    \"lon\": -0.11,\r\n    \"tz_id\": \"Europe/London\",\r\n    \"localtime_epoch\": 1690151352,\r\n    \"localtime\": \"2023-07-23 23:29\"\r\n  },\r\n  \"current\": {\r\n    \"temp_c\": 17,\r\n    \"temp_f\": 62.6,\r\n    \"condition\": {},\r\n    \"uv\": 1\r\n  },\r\n  \"astronomy\": {\r\n    \"astro\": {\r\n      \"sunrise\": \"04:47 AM\",\r\n      \"sunset\": \"09:21 PM\",\r\n      \"moonrise\": \"07:57 PM\",\r\n      \"moonset\": \"02:21 AM\",\r\n      \"moon_phase\": \"Waxing Gibbous\",\r\n      \"moon_illumination\": \"92\",\r\n      \"is_moon_up\": 1,\r\n      \"is_sun_up\": 0\r\n    }\r\n  }\r\n}";

            var token = JToken.Parse(json);

            return token.ToString();
        }
        public static string PrepareNoResultJsonObject()
        {
            const string json = "{\r\n  \"error\": {\r\n    \"code\": 1006,\r\n    \"message\": \"No matching location found.\"\r\n  }\r\n}";

            var token = JToken.Parse(json);

            return token.ToString();
        }
    }
}
