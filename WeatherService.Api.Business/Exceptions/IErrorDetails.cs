using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.Api.Business.Exceptions
{
    internal interface IErrorDetails
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
