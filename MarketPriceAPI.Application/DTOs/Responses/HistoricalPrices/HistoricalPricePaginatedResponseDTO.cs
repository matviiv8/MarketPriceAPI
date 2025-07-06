using MarketPriceAPI.Application.DTOs.Responses.Base;

namespace MarketPriceAPI.Application.DTOs.Responses.HistoricalPrices
{
    public class HistoricalPricePaginatedResponseDTO : PaginatedResultResponseDTO<HistoricalPriceResponseDTO>
    {
        public string Symbol { get; set; }
    }
}
