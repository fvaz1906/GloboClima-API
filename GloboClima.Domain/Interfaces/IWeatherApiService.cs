using GloboClima.Domain.Entities;

namespace GloboClima.Domain.Interfaces
{
    public interface IWeatherApiService
    {
        Task<WeatherData?> GetWeatherAsync(string city);
    }
}
