using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Serilog;
using WeatherService.Api;
using WeatherService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Configure Api versioning
builder.Services.ConfigureApiVersioning();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cross Origin Resource Sharing settings
builder.Services.ConfigureCors();

//Configure all custom services
builder.Services.ConfigureServices();

//Add HttpClient to access Weather Api
builder.Services.AddHttpClient();

var app = builder.Build();

//Configure logging
app.ConfigureLogging();

//Configure all custom middleware
app.UseExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
