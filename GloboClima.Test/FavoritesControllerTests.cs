using GloboClima.Application.Controllers;
using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;
using GloboClima.Infra.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace GloboClima.Test
{
    public class FavoritesControllerTests
    {
        private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
        private readonly FavoriteController _controller;

        public FavoritesControllerTests()
        {
            _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
            _controller = new FavoriteController(_favoriteRepositoryMock.Object);

            // Simulando um usuário autenticado com um userId específico
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, "user-123")
            }, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task ListFavorites_ReturnsOk_WithListOfFavorites()
        {
            // Arrange
            var userId = "user-123";
            var favorites = new List<Favorite>
        {
            new Favorite { Id = "fav-1", City = "Favorite 1", Country = "Favorite 1", UserId = userId },
            new Favorite { Id = "fav-2", City = "Favorite 2", Country = "Favorite 1", UserId = userId }
        };
            _favoriteRepositoryMock.Setup(repo => repo.GetFavoritesByUserAsync(userId))
                .ReturnsAsync(favorites);

            // Act
            var result = await _controller.ListFavorites();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFavorites = Assert.IsType<List<Favorite>>(okResult.Value);
            Assert.Equal(2, returnedFavorites.Count);
        }

        [Fact]
        public async Task SaveFavorite_ReturnsOk_WhenFavoriteIsSaved()
        {
            // Arrange
            var favorite = new Favorite { Id = "fav-1", City = "Favorite 1", Country = "Favorite 1" };
            _favoriteRepositoryMock.Setup(repo => repo.SaveFavoriteAsync(It.IsAny<Favorite>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SaveFavorite(favorite);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFavorite = Assert.IsType<Favorite>(okResult.Value);
            Assert.Equal("fav-1", returnedFavorite.Id);
            Assert.Equal("Favorite 1", returnedFavorite.City);
            Assert.Equal("Favorite 1", returnedFavorite.Country);
        }

        [Fact]
        public async Task DeleteFavorite_ReturnsOk_WhenFavoriteIsDeleted()
        {
            // Arrange
            var userId = "user-123";
            var locationId = "fav-1";
            _favoriteRepositoryMock.Setup(repo => repo.DeleteFavoriteAsync(userId, locationId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteFavorite(locationId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
