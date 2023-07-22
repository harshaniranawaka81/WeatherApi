using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.Api.Domain
{
    internal class RealTimeWeather : IRealTimeWeather   
    {
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public DateTime LocalTime { get; set; }
        public decimal Temperature { get; set; }
    }
}
