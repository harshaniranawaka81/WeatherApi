using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using WeatherService.Api.Business.Factories;
using WeatherService.Api.Business.Services;
using WeatherService.Api.Business.Strategies;
using Xunit;

namespace WeatherService.Api.Business.UnitTests.Factories
{
    public class WeatherStrategyFactoryTest
    {
        [Fact]
        public void Create_ApiVersionIs1_ReturnsBaseWeatherStrategy()
        {
            //Arrange
            var weatherApiService = new Mock<IWeatherApiService>();
            var weatherStrategyFactory = new WeatherStrategyFactory(weatherApiService.Object);

            //Act
            var result = weatherStrategyFactory.Create(1);

            //Assert
            Assert.IsType<BaseWeatherStrategy>(result);
        }


        [Fact]
        public void Create_ApiVersionIs2_ReturnsWeatherStrategyWithAstronomy()
        {
            //Arrange
            var weatherApiService = new Mock<IWeatherApiService>();
            var weatherStrategyFactory = new WeatherStrategyFactory(weatherApiService.Object);

            //Act
            var result = weatherStrategyFactory.Create(2);

            //Assert
            Assert.IsType<WeatherStrategyWithAstronomy>(result);
        }


        [Fact]
        public void Create_ApiVersionIs0_ReturnsBaseWeatherStrategy()
        {
            //Arrange
            var weatherApiService = new Mock<IWeatherApiService>();
            var weatherStrategyFactory = new WeatherStrategyFactory(weatherApiService.Object);

            //Act
            var result = weatherStrategyFactory.Create(0);

            //Assert
            Assert.IsType<BaseWeatherStrategy>(result);
        }
    }
}
