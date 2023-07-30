using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using WeatherService.Api.Business.Clients;
using WeatherService.Api.Business.Factories;
using WeatherService.Api.Business.Services;
using WeatherService.Api.Business.Strategies;
using WeatherService.Api.Controllers;
using Xunit;
using WeatherService.Api.Business.DTO;
using System.IO;
using System.Net;
using Moq.Protected;
using System.Threading;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace WeatherService.Api.UnitTests
{
    public class WeatherControllerTest
    {
        private IConfiguration CreateConfigMock()
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

        private HttpClient CreateHttpClientMock(HttpResponseMessage httpResponseMessage)
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

        private string PrepareOkJsonObject_GetRealTimeWeather()
        {
            const string json = "{\r\n  \"location\": {\r\n    \"name\": \"London\",\r\n    \"region\": \"City of London, Greater London\",\r\n    \"country\": \"United Kingdom\",\r\n    \"lat\": 51.52,\r\n    \"lon\": -0.11,\r\n    \"tz_id\": \"Europe/London\",\r\n    \"localtime_epoch\": 1690135710,\r\n    \"localtime\": \"2023-07-23 19:08\"\r\n  },\r\n  \"current\": {\r\n    \"temp_c\": 22,\r\n    \"temp_f\": 71.6,\r\n    \"condition\": {},\r\n    \"uv\": 6\r\n  }\r\n}";
          
            var token = JToken.Parse(json);

            return token.ToString();
        }

        private string PrepareOkJsonObject_GetRealTimeWeatherWithAstronomy()
        {
            const string json = "{\r\n  \"location\": {\r\n    \"name\": \"London\",\r\n    \"region\": \"City of London, Greater London\",\r\n    \"country\": \"United Kingdom\",\r\n    \"lat\": 51.52,\r\n    \"lon\": -0.11,\r\n    \"tz_id\": \"Europe/London\",\r\n    \"localtime_epoch\": 1690151352,\r\n    \"localtime\": \"2023-07-23 23:29\"\r\n  },\r\n  \"current\": {\r\n    \"temp_c\": 17,\r\n    \"temp_f\": 62.6,\r\n    \"condition\": {},\r\n    \"uv\": 1\r\n  },\r\n  \"astronomy\": {\r\n    \"astro\": {\r\n      \"sunrise\": \"04:47 AM\",\r\n      \"sunset\": \"09:21 PM\",\r\n      \"moonrise\": \"07:57 PM\",\r\n      \"moonset\": \"02:21 AM\",\r\n      \"moon_phase\": \"Waxing Gibbous\",\r\n      \"moon_illumination\": \"92\",\r\n      \"is_moon_up\": 1,\r\n      \"is_sun_up\": 0\r\n    }\r\n  }\r\n}";

            var token = JToken.Parse(json);

            return token.ToString();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("unknown-city")]
        public async Task GetRealTimeWeather_NoInputParam_ThrowsException(string city)
        {
            //Arrange 
            var loggerWeatherApiService = new Mock<ILogger<WeatherApiService>>();
            var loggerWeatherController = new Mock<ILogger<WeatherController>>();

            var config = CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            var client = CreateHttpClientMock(httpResponseMessage);
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var weatherApiClient = new Mock<WeatherApiClient>(httpClientFactory.Object, config);
            var weatherApiService =
                new WeatherApiService(config, loggerWeatherApiService.Object, weatherApiClient.Object);
            var weatherStrategyFactory = new WeatherStrategyFactory(weatherApiService);

            var weatherController = new WeatherController(loggerWeatherController.Object, weatherStrategyFactory);

            //Act
            var result = weatherController.GetRealTimeWeather(city);

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await weatherController.GetRealTimeWeather(string.Empty));
        }

        [Fact]
        public async Task GetRealTimeWeather_CorrectInputParam_ReturnsOK()
        {
            //Arrange 
            var loggerWeatherApiService = new Mock<ILogger<WeatherApiService>>();
            var loggerWeatherController = new Mock<ILogger<WeatherController>>();

            var config = CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(PrepareOkJsonObject())
            };

            var client = CreateHttpClientMock(httpResponseMessage);
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var weatherApiClient = new Mock<WeatherApiClient>(httpClientFactory.Object, config);
            var weatherApiService = new WeatherApiService(config, loggerWeatherApiService.Object, weatherApiClient.Object);
            var weatherStrategyFactory = new WeatherStrategyFactory(weatherApiService);

            var weatherController = new WeatherController(loggerWeatherController.Object, weatherStrategyFactory);

            //Act
            var result = weatherController.GetRealTimeWeather(city: "London");

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("unknown-city")]
        public async Task GetRealTimeWeatherWithAstronomy_NoInputParam_ThrowsException()
        {
            //Arrange 
            var loggerWeatherApiService = new Mock<ILogger<WeatherApiService>>();
            var loggerWeatherController = new Mock<ILogger<WeatherController>>();

            var config = CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            var client = CreateHttpClientMock(httpResponseMessage);
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var weatherApiClient = new Mock<WeatherApiClient>(httpClientFactory.Object, config);
            var weatherApiService =
                new WeatherApiService(config, loggerWeatherApiService.Object, weatherApiClient.Object);
            var weatherStrategyFactory = new WeatherStrategyFactory(weatherApiService);

            var weatherController = new WeatherController(loggerWeatherController.Object, weatherStrategyFactory);

            //Act
            var result = weatherController.GetRealTimeWeatherWithAstronomy(string.Empty);

            //Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await weatherController.GetRealTimeWeather(string.Empty));
        }

        [Fact]
        public async Task GetRealTimeWeatherWithAstronomy_CorrectInputParam_ReturnsOK()
        {
            //Arrange 
            var loggerWeatherApiService = new Mock<ILogger<WeatherApiService>>();
            var loggerWeatherController = new Mock<ILogger<WeatherController>>();

            var config = CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(PrepareOkJsonObjectForAstronomy())
            };

            var client = CreateHttpClientMock(httpResponseMessage);
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var weatherApiClient = new Mock<WeatherApiClient>(httpClientFactory.Object, config);
            var weatherApiService = new WeatherApiService(config, loggerWeatherApiService.Object, weatherApiClient.Object);
            var weatherStrategyFactory = new WeatherStrategyFactory(weatherApiService);

            var weatherController = new WeatherController(loggerWeatherController.Object, weatherStrategyFactory);

            //Act
            var result = weatherController.GetRealTimeWeatherWithAstronomy(city: "London");

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}
