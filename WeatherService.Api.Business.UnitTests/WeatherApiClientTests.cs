using Moq;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Api.Business.Clients;
using Xunit;
using System.ComponentModel.DataAnnotations;
using WeatherService.Api.Controllers;

namespace WeatherService.Api.Business.UnitTests
{
    public class WeatherApiClientTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("wrong-url")]
        public async Task GetAsync_InvalidUrl_ThrowsException(string url)
        {
            //Arrange
            var config = TestData.CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(TestData.PrepareOkJsonObject_GetRealTimeWeather())
            };

            var client = TestData.CreateHttpClientMock(httpResponseMessage);
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var weatherApiClient = new WeatherApiClient(httpClientFactory.Object, config);

            //Act and Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await weatherApiClient.GetAsync(url));
        }

        [Theory]
        [InlineData("http://api.weatherapi.com/v1/current.json")]
        public async Task GetAsync_ValidUrl_ReturnsOk(string url)
        {
            //Arrange
            var config = TestData.CreateConfigMock();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(TestData.PrepareOkJsonObject_GetRealTimeWeather())
            };

            var client = TestData.CreateHttpClientMock(httpResponseMessage);
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var weatherApiClient = new WeatherApiClient(httpClientFactory.Object, config);

            //Act
            var result = await weatherApiClient.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.Key);
        }
    }
}