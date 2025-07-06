using MarketPriceAPI.Domain.Models;

namespace MarketPriceAPI.Domain.Interfaces
{
    public interface IAssetRepository : IBaseRepository<Asset, string> 
    {
        Task<IEnumerable<Asset>> GetFilteredAllAsync(string symbol = null, string kind = null);
    }
}
