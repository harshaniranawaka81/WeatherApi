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
    public class BaseWeatherStrategy : IBaseWeatherStrategy
    {
        private readonly IWeatherApiService _weatherApiService;

        public BaseWeatherStrategy(IWeatherApiService weatherApiService)
        {
            _weatherApiService = weatherApiService;
        }

        public virtual async Task<KeyValuePair<HttpStatusCode, string>> GetWeatherInformationAsync(string city)
        {
            var result = await _weatherApiService.GetRealTimeWeatherAsync(city);

            if (result.Key != HttpStatusCode.OK) return result;

            dynamic json = JObject.Parse(result.Value);
            var realtimeWeatherObj = new RealtimeWeather()
            {
                City = json.location.name,
                Region = json.location.region,
                Country = json.location.country,
                LocalTime = json.location.localtime,
                Temperature = json.current.temp_c,
            };

            return new KeyValuePair<HttpStatusCode, string>(HttpStatusCode.OK, JsonConvert.SerializeObject(realtimeWeatherObj));

        }
    }
}
