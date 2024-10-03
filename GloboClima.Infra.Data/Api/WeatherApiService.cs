using GloboClima.Domain.Entities;
using System.Net.Http.Json;

namespace GloboClima.Infra.Data.Api
{
    public class WeatherApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "ef48e1597acb5cda0bac0153dd99d689";

        public WeatherApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherData?> GetWeatherAsync(string city)
        {
            try
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var weatherData = await response.Content.ReadFromJsonAsync<WeatherData>();
                return weatherData;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return null;
            }
        }
    }
}
