using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeatherService.Api.Business.DTO;
using WeatherService.Api.Business.Services;

namespace WeatherService.Api.Business.Strategies
{
    public class WeatherStrategyWithAstronomy : BaseWeatherStrategy
    {
        private readonly IWeatherApiService _weatherApiService;

        public WeatherStrategyWithAstronomy(IWeatherApiService weatherApiService) : base(weatherApiService)
        {
            _weatherApiService = weatherApiService;
        }

        public override async Task<KeyValuePair<HttpStatusCode, string>> GetWeatherInformationAsync(string city)
        {
            var weather = await _weatherApiService.GetRealTimeWeatherAsync(city);

            if (weather.Key != HttpStatusCode.OK) return weather;

            var astronomy = await _weatherApiService.GetRealTimeWeatherAsync(city);

            if (astronomy.Key != HttpStatusCode.OK) return astronomy;

            dynamic jsonWeather = JObject.Parse(weather.Value);
            dynamic jsonAstonomy = JObject.Parse(astronomy.Value);

            var realtimeWeatherObj = new RealtimeWeatherWithAstronomy()
            {
                City = jsonWeather.location.name,
                Region = jsonWeather.location.region,
                Country = jsonWeather.location.country,
                LocalTime = jsonWeather.location.localtime,
                Temperature = jsonWeather.current.temp_c,

                Sunrise = jsonAstonomy.current.temp_c,
                Sunset = jsonAstonomy.current.temp_c,
            };

            return new KeyValuePair<HttpStatusCode, string>(HttpStatusCode.OK, JsonConvert.SerializeObject(realtimeWeatherObj));
        }
    }
}
