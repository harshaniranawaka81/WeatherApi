using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.Api.Business.Strategies
{
    public interface IBaseWeatherStrategy
    {
        Task<KeyValuePair<HttpStatusCode, string>> GetWeatherInformationAsync(string city);
    }
}
