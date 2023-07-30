using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService.Api.Business.Strategies;

namespace WeatherService.Api.Business.Factories
{
    public interface IWeatherStrategyFactory
    {
        public IBaseWeatherStrategy Create(int apiVersion);
    }
}
