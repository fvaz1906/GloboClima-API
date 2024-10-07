using GloboClima.Application.Controllers;
using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;
using GloboClima.Domain.Models;
using GloboClima.Infra.CrossCutting.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GloboClima.Test
{
    public class AuthenticateTests
    {
        private readonly Mock<IBaseService<User>> _baseServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthController _controller;

        public AuthenticateTests()
        {
            _baseServiceMock = new Mock<IBaseService<User>>();
            _configurationMock = new Mock<IConfiguration>();

            _controller = new AuthController(_baseServiceMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Authenticate_ReturnsBadRequest_ForInvalidEmail()
        {
            // Arrange
            var invalidUser = new User { Email = "invalid-email", Password = "password123" };

            // Act
            var result = await _controller.Authenticate(invalidUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("E-mail Inválido", badRequestResult.Value);
        }

        [Fact]
        public async Task Authenticate_ReturnsBadRequest_WhenUserNotFound()
        {
            // Arrange
            var validUser = new User { Email = "test@example.com", Password = "password123" };

            _baseServiceMock.Setup(service => service.GetAsync())
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _controller.Authenticate(validUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Usuário inexistente", badRequestResult.Value);
        }

        [Fact]
        public async Task Authenticate_ReturnsOk_ForValidCredentials()
        {
            // Arrange
            var validUser = new User
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Simulando um usuário existente no sistema com as mesmas credenciais
            var storedUser = new User
            {
                Email = "test@example.com",
                Password = SHA2.GenerateHash("password123"),
                Name = "Test User",
                Id = 1 // Adicionando o ID necessário para a geração do token
            };

            // Mock do serviço de usuários
            _baseServiceMock.Setup(service => service.GetAsync())
                .ReturnsAsync(new List<User> { storedUser });

            // Mock da configuração (IConfiguration) para as chaves JWT
            _configurationMock.Setup(config => config["Jwt:Key"])
                .Returns("supersecretkeywith32characters1234"); // Substitua pelo valor da chave que usaria em produção
            _configurationMock.Setup(config => config["Jwt:ExpiresInMinutes"])
                .Returns("60");
            _configurationMock.Setup(config => config["Jwt:Issuer"])
                .Returns("myIssuer");
            _configurationMock.Setup(config => config["Jwt:Audience"])
                .Returns("myAudience");

            // Act
            var result = await _controller.Authenticate(validUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var token = Assert.IsType<TokenModel>(okResult.Value);
            Assert.Equal(storedUser.Email, token.Email);
        }
    }
}
