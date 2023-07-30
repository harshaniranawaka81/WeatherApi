using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WeatherService.Api.Business.Services
{
    public interface IWeatherApiService
    {
        Task<KeyValuePair<HttpStatusCode, string>> GetRealTimeWeatherAsync(string city);
        Task<KeyValuePair<HttpStatusCode, string>> GetAstonomyAsync(string city);
    }
}
