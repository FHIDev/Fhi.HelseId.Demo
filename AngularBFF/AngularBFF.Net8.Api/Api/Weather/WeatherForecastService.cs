using Refit;
using System.Text.Json;

namespace AngularBFF.Net8.Api.Weather
{
    public record WeatherForcastModel(DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);

    public interface IWeatherForecastService
    {
        public Task<IEnumerable<WeatherForcastModel>?> GetWeather();
    }

    /// <summary>
    /// Calling external API to get weather forcast
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <param name="tokenService"></param>
    public class WeatherForecastService : IWeatherForecastService
    {
        public WeatherForecastService(IHttpClientFactory factory)
        {
            Factory = factory;
        }
        public IHttpClientFactory Factory { get; }

        public async Task<IEnumerable<WeatherForcastModel>?> GetWeather()
        {
            var client = Factory.CreateClient("WeatherApi");
            var response = await client.GetAsync("api/v1/weatherforecast");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<WeatherForcastModel>>(content);
        }
    }

    public class WeatherForecastServiceWithRefit : IWeatherForecastService
    {
        public WeatherForecastServiceWithRefit(IWeatherForcastApi weatherForcastApi)
        {
            WeatherForcastApi = weatherForcastApi;
        }

        public IWeatherForcastApi WeatherForcastApi { get; }

        public async Task<IEnumerable<WeatherForcastModel>?> GetWeather()
        {
            var response = await WeatherForcastApi.GetWeatherForcast();
            return response.Content;
        }
    }

    public interface IWeatherForcastApi
    {
        [Get("/api/v1/weatherforecast")]
        Task<ApiResponse<IEnumerable<WeatherForcastModel>>> GetWeatherForcast();
    }
}
