using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GloboClima.Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteController(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        /// <summary>
        /// Obtém a lista de favoritos do usuário.
        /// </summary>
        /// <returns>Uma lista de favoritos.</returns>

        [HttpGet("list")]
        public async Task<IActionResult> ListFavorites()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var favorites = await _favoriteRepository.GetFavoritesByUserAsync(userId);
            return Ok(favorites);
        }

        /// <summary>
        /// Adiciona um novo favorito.
        /// </summary>
        /// <param name="favorite">O favorito a ser adicionado.</param>
        /// <returns>Retorna o status da operação.</returns>
        [HttpPost("save")]
        public async Task<IActionResult> SaveFavorite([FromBody] Favorite favorite)
        {
            favorite.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _favoriteRepository.SaveFavoriteAsync(favorite);
            return Ok(favorite);
        }

        /// <summary>
        /// Remove um favorito com base no identificador.
        /// </summary>
        /// <param name="hashId">O identificador do favorito a ser removido.</param>
        /// <returns>A lista de favoritos atualizada após a remoção.</returns>
        [HttpDelete("delete/{locationId}")]
        public async Task<IActionResult> DeleteFavorite(string locationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _favoriteRepository.DeleteFavoriteAsync(userId, locationId);
            return Ok();
        }

    }
}
