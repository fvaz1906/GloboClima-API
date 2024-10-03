using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;
using GloboClima.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GloboClima.Infra.Data.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<TEntity>> SelectAsync() => await _context.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> SelectAsync(int id)
        {
            return await _context.Set<TEntity>()
                                 .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task InsertAsync(TEntity obj)
        {
            await _context.Set<TEntity>().AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity obj)
        {
            var localEntity = _context.Set<TEntity>().FirstOrDefault(e => e.Id == obj.Id);
            if (localEntity != null)
            {
                _context.Entry(localEntity).State = EntityState.Detached;
            }

            obj.CreateDate = localEntity.CreateDate;

            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await SelectAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

    }
}
