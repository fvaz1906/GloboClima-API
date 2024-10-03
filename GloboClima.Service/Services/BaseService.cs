using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;

namespace GloboClima.Service.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        private readonly IBaseRepository<TEntity> _baseRepository;

        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<IList<TEntity>> GetAsync() => await _baseRepository.SelectAsync();

        public async Task<TEntity> GetByIdAsync(int id) => await _baseRepository.SelectAsync(id);

        public async Task<TEntity> AddAsync(TEntity obj)
        {
            await _baseRepository.InsertAsync(obj);
            return obj;
        }

        public async Task<TEntity> UpdateAsync(TEntity obj)
        {
            await _baseRepository.UpdateAsync(obj);
            return obj;
        }

        public async Task DeleteAsync(int id) => await _baseRepository.DeleteAsync(id);

    }
}
