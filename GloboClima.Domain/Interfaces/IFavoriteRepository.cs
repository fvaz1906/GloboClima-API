using GloboClima.Domain.Entities;

namespace GloboClima.Domain.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<List<Favorite>> GetFavoritesByUserAsync(string userId);
        Task SaveFavoriteAsync(Favorite favorite);
        Task DeleteFavoriteAsync(string userId, string locationId);
    }
}
