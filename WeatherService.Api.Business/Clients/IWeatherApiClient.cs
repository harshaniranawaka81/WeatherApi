using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace WeatherService.Api.Business.Clients
{
    public interface IWeatherApiClient
    {
        Task<KeyValuePair<HttpStatusCode, string>> GetAsync(string url);
    }

}
