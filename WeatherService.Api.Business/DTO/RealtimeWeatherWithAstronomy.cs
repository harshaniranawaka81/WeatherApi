﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.Api.Business.DTO
{
    public class RealtimeWeatherWithAstronomy : RealtimeWeather
    {
        public string? Sunrise { get; set; }
        public string? Sunset { get; set; }
    }
}
