using MarketPriceAPI.Application.Interfaces.Services;

namespace MarketPriceAPI.Application.Services
{
    public class PaginationService<T> : IPaginationService<T>
    {
        public async Task<(IEnumerable<T> Items, int TotalPages)> PaginateWithTotalPages(IEnumerable<T> items, int page, int pageSize)
        {
            var totalItems = items.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var paginatedItems = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (paginatedItems, totalPages);
        }
    }
}
