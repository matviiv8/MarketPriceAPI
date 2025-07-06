namespace MarketPriceAPI.Application.Interfaces.Services
{
    public interface IPaginationService<T>
    {
        Task<(IEnumerable<T> Items, int TotalPages)> PaginateWithTotalPages(IEnumerable<T> items, int page, int pageSize);
    }
}
