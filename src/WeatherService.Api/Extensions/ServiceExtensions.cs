using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeatherService.Api.Business.Clients;
using WeatherService.Api.Business.Factories;
using WeatherService.Api.Business.Services;
using WeatherService.Api.Business.Strategies;

namespace WeatherService.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Weather API V1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Harshani Ranawaka",
                        Email = "harshaniranawaka@gmail.com",
                        Url = new Uri("https://github.com/harshaniranawaka81")
                    }
                });

                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Weather API V2",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Harshani Ranawaka",
                        Email = "harshaniranawaka@gmail.com",
                        Url = new Uri("https://github.com/harshaniranawaka81")
                    }
                });

                // generate the XML docs that'll drive the swagger docs
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

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
            services.AddScoped<IWeatherStrategyFactory, WeatherStrategyFactory>();
            services.AddScoped<IBaseWeatherStrategy, BaseWeatherStrategy>();
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
