using GloboClima.Application.Controllers;
using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GloboClima.Test
{
    public class CountryControllerTests
    {
        private readonly Mock<ICountryApiService> _countryApiServiceMock;
        private readonly CountryController _controller;

        public CountryControllerTests()
        {
            _countryApiServiceMock = new Mock<ICountryApiService>();
            _controller = new CountryController(_countryApiServiceMock.Object);
        }

        [Fact]
        public async Task GetCountry_ReturnsOk_WhenCountryIsFound()
        {
            // Arrange
            var countryName = "Brazil";
            var countryData = new[]
            {
            new CountryData { Independent = true, Region = "Americas", Population = 211000000 }
        };
            _countryApiServiceMock.Setup(service => service.GetCountryAsync(countryName))
                .ReturnsAsync(countryData);

            // Act
            var result = await _controller.GetCountry(countryName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCountry = Assert.IsType<CountryData>(okResult.Value);
            Assert.Equal(true, returnedCountry.Independent);
            Assert.Equal("Americas", returnedCountry.Region);
        }

        [Fact]
        public async Task GetCountry_ReturnsNotFound_WhenCountryIsNotFound()
        {
            // Arrange
            var countryName = "Atlantis"; // Um nome de país que provavelmente não existe.
            _countryApiServiceMock.Setup(service => service.GetCountryAsync(countryName))
                .ReturnsAsync(new CountryData[0]); // Retorna um array vazio para simular que o país não foi encontrado.

            // Act
            var result = await _controller.GetCountry(countryName);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Informações do país não encontradas.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetCountry_ReturnsNotFound_WhenCountryDataIsNull()
        {
            // Arrange
            var countryName = "UnknownCountry";
            _countryApiServiceMock.Setup(service => service.GetCountryAsync(countryName))
                .ReturnsAsync((CountryData[])null); // Simula um retorno nulo do serviço.

            // Act
            var result = await _controller.GetCountry(countryName);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Informações do país não encontradas.", notFoundResult.Value);
        }
    }
}
