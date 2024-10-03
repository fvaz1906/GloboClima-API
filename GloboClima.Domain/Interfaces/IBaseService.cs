using GloboClima.Domain.Entities;

namespace GloboClima.Domain.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        Task<IList<TEntity>> GetAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity obj);
        Task<TEntity> UpdateAsync(TEntity obj);
        Task DeleteAsync(int id);
    }
}
