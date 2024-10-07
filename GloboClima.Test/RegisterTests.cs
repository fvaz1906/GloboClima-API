using GloboClima.Application.Controllers;
using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GloboClima.Test
{
    public class RegisterTests
    {
        private readonly Mock<IBaseService<User>> _baseServiceMock;
        private readonly AuthController _controller;

        public RegisterTests()
        {
            _baseServiceMock = new Mock<IBaseService<User>>();
            _controller = new AuthController(_baseServiceMock.Object, null);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_ForInvalidEmail()
        {
            // Arrange
            var invalidUser = new User { Email = "invalid-email", Password = "password123" };

            // Act
            var result = await _controller.Register(invalidUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("E-mail Inválido", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserAlreadyExists()
        {
            // Arrange
            var newUser = new User { Email = "test@example.com", Password = "password123" };
            var existingUsers = new List<User>
        {
            new User { Email = "test@example.com", Password = "hashedPassword" }
        };

            _baseServiceMock.Setup(service => service.GetAsync())
                .ReturnsAsync(existingUsers);

            // Act
            var result = await _controller.Register(newUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Usuário existente", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsSuccessfullyRegistered()
        {
            // Arrange
            var newUser = new User { Email = "test@example.com", Password = "password123" };

            _baseServiceMock.Setup(service => service.GetAsync())
                .ReturnsAsync(new List<User>());

            // Ajuste para que o AddAsync retorne um Task<User> com o usuário criado
            _baseServiceMock.Setup(service => service.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(newUser);

            // Act
            var result = await _controller.Register(newUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<User>(okResult.Value);
            Assert.Equal(newUser.Email, user.Email);
        }
    }
}
