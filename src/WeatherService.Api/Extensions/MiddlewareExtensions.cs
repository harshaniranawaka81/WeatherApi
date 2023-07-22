using WeatherService.Api.Business.Middleware;
using WeatherService.Api.Business.Services;

namespace WeatherService.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }

    }
}
