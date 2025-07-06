using System.Text.Json.Serialization;

namespace MarketPriceAPI.Application.DTOs.Responses.HistoricalPrices
{
    public class HistoricalPriceApiResponseDTO
    {
        [JsonPropertyName("t")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("o")]
        public double OpenPrice { get; set; }

        [JsonPropertyName("h")]
        public double HighPrice { get; set; }

        [JsonPropertyName("l")]
        public double LowPrice { get; set; }

        [JsonPropertyName("c")]
        public double ClosePrice { get; set; }

        [JsonPropertyName("v")]
        public long Volume { get; set; }
    }
}
