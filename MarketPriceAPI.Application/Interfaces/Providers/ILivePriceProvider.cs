using MarketPriceAPI.Application.DTOs.Responses.LivePrices;

namespace MarketPriceAPI.Application.Interfaces.Providers
{
    public interface ILivePriceProvider
    {
        Task<LivePriceApiResponseDTO> GetLiveMarketPriceAsync(string assetId);
    }
}
