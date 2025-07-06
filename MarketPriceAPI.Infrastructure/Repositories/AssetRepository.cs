using MarketPriceAPI.Domain.Interfaces;
using MarketPriceAPI.Domain.Models;
using MarketPriceAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceAPI.Infrastructure.Repositories
{
    public class AssetRepository : BaseRepository<Asset, string>, IAssetRepository
    {
        public AssetRepository(MarketPriceDbContext context) : base(context) { }

        public async Task<IEnumerable<Asset>> GetFilteredAllAsync(string symbol = null, string kind = null)
        {
            var assets = _context.Assets.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(symbol))
            {
                assets = assets.Where(asset => asset.Symbol.ToLower().Contains(symbol.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(kind))
            {
                assets = assets.Where(asset => asset.Kind.ToLower().Contains(kind.ToLower()));
            }

            return assets;
        }
    }
}
