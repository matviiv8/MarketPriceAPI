using MarketPriceAPI.Application.DTOs.Requests.HistoricalPrices;
using MarketPriceAPI.Application.DTOs.Responses.HistoricalPrices;
using MarketPriceAPI.Application.DTOs.Responses.LivePrices;

namespace MarketPriceAPI.Application.Interfaces.Services
{
    public interface IMarketPriceService
    {
        Task<HistoricalPricePaginatedResponseDTO> GetAssetHistoricalPricesAsync(string assetId, HistoricalPriceRequestDTO requestDto);
        Task<LastLivePriceResponseDTO> GetLastLivePriceAsync(string assetId);
    }
}
