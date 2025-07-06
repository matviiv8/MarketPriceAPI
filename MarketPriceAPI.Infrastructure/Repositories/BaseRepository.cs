using MarketPriceAPI.Domain.Interfaces;
using MarketPriceAPI.Domain.Models;
using MarketPriceAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceAPI.Infrastructure.Repositories
{
    public abstract class BaseRepository<T, TKey> : IBaseRepository<T, TKey> where T : BaseEntity<TKey>
    {
        protected readonly MarketPriceDbContext _context;

        public BaseRepository(MarketPriceDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(TKey id)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public virtual async Task AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var existingEntity = await _context.Set<T>().FindAsync(entity.Id);
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
