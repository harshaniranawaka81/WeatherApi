using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WeatherService.Api.Business.Clients
{
    public interface IWeatherApiClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }

}
