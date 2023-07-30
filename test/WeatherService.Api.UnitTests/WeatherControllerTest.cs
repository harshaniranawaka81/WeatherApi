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
using WeatherService.Api.Business.UnitTests;
using static System.Net.Mime.MediaTypeNames;

namespace WeatherService.Api.UnitTests
{
    public class WeatherControllerTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("unknown-city")]
        public async Task GetRealTimeWeather_InvalidInputParam_ThrowsException(string city)
        {
            //Arrange 
            var loggerWeatherApiService = new Mock<ILogger<WeatherApiService>>();
            var loggerWeatherController = new Mock<ILogger<WeatherController>>();

            var config = TestData.CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            var client = TestData.CreateHttpClientMock(httpResponseMessage);
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var weatherApiClient = new Mock<WeatherApiClient>(httpClientFactory.Object, config);
            var weatherApiService =
                new WeatherApiService(config, loggerWeatherApiService.Object, weatherApiClient.Object);
            var weatherStrategyFactory = new WeatherStrategyFactory(weatherApiService);

            var weatherController = new WeatherController(loggerWeatherController.Object, weatherStrategyFactory);

            //Act and Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await weatherController.GetRealTimeWeather(string.Empty));
        }

        [Fact]
        public async Task GetRealTimeWeather_CorrectInputParam_ReturnsOK()
        {
            //Arrange 
            var loggerWeatherApiService = new Mock<ILogger<WeatherApiService>>();
            var loggerWeatherController = new Mock<ILogger<WeatherController>>();

            var config = TestData.CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(TestData.PrepareOkJsonObject_GetRealTimeWeather())
            };

            var client = TestData.CreateHttpClientMock(httpResponseMessage);
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
        public async Task GetRealTimeWeatherWithAstronomy_InvalidInputParam_ThrowsException(string city)
        {
            //Arrange 
            var loggerWeatherApiService = new Mock<ILogger<WeatherApiService>>();
            var loggerWeatherController = new Mock<ILogger<WeatherController>>();

            var config = TestData.CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            var client = TestData.CreateHttpClientMock(httpResponseMessage);
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var weatherApiClient = new Mock<WeatherApiClient>(httpClientFactory.Object, config);
            var weatherApiService =
                new WeatherApiService(config, loggerWeatherApiService.Object, weatherApiClient.Object);
            var weatherStrategyFactory = new WeatherStrategyFactory(weatherApiService);

            var weatherController = new WeatherController(loggerWeatherController.Object, weatherStrategyFactory);

            //Act and Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await weatherController.GetRealTimeWeatherWithAstronomy(string.Empty));
        }

        [Fact]
        public async Task GetRealTimeWeatherWithAstronomy_CorrectInputParam_ReturnsOK()
        {
            //Arrange 
            var loggerWeatherApiService = new Mock<ILogger<WeatherApiService>>();
            var loggerWeatherController = new Mock<ILogger<WeatherController>>();

            var config = TestData.CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(TestData.PrepareOkJsonObject_GetRealTimeWeatherWithAstronomy())
            };

            var client = TestData.CreateHttpClientMock(httpResponseMessage);
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
