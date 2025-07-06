using MarketPriceAPI.Application.DTOs.Requests.HistoricalPrices;
using MarketPriceAPI.Application.DTOs.Responses.HistoricalPrices;

namespace MarketPriceAPI.Application.Interfaces.Providers
{
    public interface IHistoricalPriceProvider
    {
        Task<IEnumerable<HistoricalPriceResponseDTO>> GetHistoricalMarketPricesAsync(string assetId, HistoricalPriceRequestDTO requestDto);
    }
}
