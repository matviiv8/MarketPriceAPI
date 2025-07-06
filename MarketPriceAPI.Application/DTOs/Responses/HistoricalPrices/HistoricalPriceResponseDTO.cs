namespace MarketPriceAPI.Application.DTOs.Responses.HistoricalPrices
{
    public class HistoricalPriceResponseDTO
    {
        public DateTime Timestamp { get; set; }
        public double OpenPrice { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double ClosePrice { get; set; }
        public long Volume { get; set; }
    }
}
