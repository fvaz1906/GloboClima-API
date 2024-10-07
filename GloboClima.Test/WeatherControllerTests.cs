using GloboClima.Application.Controllers;
using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GloboClima.Test
{
    public class WeatherControllerTests
    {
        private readonly Mock<IWeatherApiService> _weatherApiServiceMock;
        private readonly WeatherController _controller;

        public WeatherControllerTests()
        {
            _weatherApiServiceMock = new Mock<IWeatherApiService>();
            _controller = new WeatherController(_weatherApiServiceMock.Object);
        }

        [Fact]
        public async Task GetWeather_ReturnsOk_WhenWeatherDataIsFound()
        {
            // Arrange
            var city = "São Paulo";
            var weatherData = new WeatherData
            {
                Name = city
            };

            _weatherApiServiceMock.Setup(service => service.GetWeatherAsync(city))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _controller.GetWeather(city);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedWeather = Assert.IsType<WeatherData>(okResult.Value);
            Assert.Equal(city, returnedWeather.Name);
        }

        [Fact]
        public async Task GetWeather_ReturnsNotFound_WhenWeatherDataIsNotFound()
        {
            // Arrange
            var city = "Atlantis"; // Simulando uma cidade que provavelmente não existe.
            _weatherApiServiceMock.Setup(service => service.GetWeatherAsync(city))
                .ReturnsAsync((WeatherData)null); // Retorna `null` para simular que não encontrou dados climáticos.

            // Act
            var result = await _controller.GetWeather(city);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Dados climáticos não encontrados para a cidade especificada.", notFoundResult.Value);
        }
    }
}
