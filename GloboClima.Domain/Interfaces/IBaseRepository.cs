using GloboClima.Domain.Entities;

namespace GloboClima.Domain.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IList<TEntity>> SelectAsync();
        Task<TEntity?> SelectAsync(int id);
        Task InsertAsync(TEntity obj);
        Task UpdateAsync(TEntity obj);
        Task DeleteAsync(int id);
    }
}
