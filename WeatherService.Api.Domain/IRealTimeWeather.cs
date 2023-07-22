namespace WeatherService.Api.Domain
{
    public interface IRealTimeWeather
    {
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public DateTime LocalTime { get; set; }
        public decimal  Temperature { get; set; }
    }
}