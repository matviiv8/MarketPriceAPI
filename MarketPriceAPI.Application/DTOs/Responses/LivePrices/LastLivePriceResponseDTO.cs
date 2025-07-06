namespace MarketPriceAPI.Application.DTOs.Responses.LivePrices
{
    public class LastLivePriceResponseDTO
    {
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public double Price { get; set; }
        public long Volume { get; set; }
        public double Change { get; set; }
        public double ChangePct { get; set; }
    }
}
