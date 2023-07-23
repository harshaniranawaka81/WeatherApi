using Microsoft.AspNetCore.Mvc.Versioning;
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
       
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"));
            });

            // Add ApiExplorer to discover versions
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
        }

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
        public static void ConfigureLogging(this WebApplication app)
        {
            var loggerFactory = app.Services.GetService<ILoggerFactory>();
            var logFilePath = app.Configuration["Logging:LogFilePath"];
            if (!string.IsNullOrEmpty(logFilePath))
                loggerFactory.AddFile(logFilePath);
        }
    }
}
