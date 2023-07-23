using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService.Api.Business.Services;
using WeatherService.Api.Business.Strategies;

namespace WeatherService.Api.Business.Factories
{
    public class WeatherStrategyFactory: IWeatherStrategyFactory
    {
        private readonly IWeatherApiService _weatherApiService;

        public WeatherStrategyFactory(IWeatherApiService weatherApiService)
        {
            _weatherApiService = weatherApiService;
        }

        public IBaseWeatherStrategy Create(int apiVersion)
        {
            IBaseWeatherStrategy weatherStrategy;

            switch (apiVersion)
            {
                case 1:
                    weatherStrategy = new BaseWeatherStrategy(_weatherApiService);
                    break;
                default:
                    weatherStrategy = new BaseWeatherStrategy(_weatherApiService);
                    break;
            }

            return weatherStrategy;
        }
    }
}
