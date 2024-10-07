using GloboClima.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherApiService _weatherApiService;

        public WeatherController(IWeatherApiService weatherApiService)
        {
            _weatherApiService = weatherApiService;
        }

        /// <summary>
        /// Obtém uma cidade.
        /// </summary>
        /// <returns>uma cidade encontrada.</returns>
        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            var weatherData = await _weatherApiService.GetWeatherAsync(city);
            if (weatherData != null)
            {
                return Ok(weatherData);
            }
            return NotFound("Dados climáticos não encontrados para a cidade especificada.");
        }
    }
}
