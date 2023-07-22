using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService.Api.Business.Clients;
using WeatherService.Api.Business.Services;

namespace WeatherService.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IWeatherApiClient, WeatherApiClient>();
            services.AddScoped<IWeatherApiService, WeatherApiService>();
        }

    }
}
