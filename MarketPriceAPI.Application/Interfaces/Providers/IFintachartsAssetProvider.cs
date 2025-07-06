using MarketPriceAPI.Domain.Models;

namespace MarketPriceAPI.Application.Interfaces.Providers
{
    public interface IFintachartsAssetProvider
    {
        Task<IEnumerable<Asset>> GetAssetsAsync();
    }
}
