using System.Collections;

namespace MarketPriceAPI.Domain.Interfaces
{
    public interface IBaseRepository<T, TKey>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(TKey id);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
