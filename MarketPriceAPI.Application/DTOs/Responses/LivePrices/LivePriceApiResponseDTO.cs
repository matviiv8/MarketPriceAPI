using System.Text.Json.Serialization;

namespace MarketPriceAPI.Application.DTOs.Responses.LivePrices
{
    public class LivePriceApiResponseDTO
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("volume")]
        public long Volume { get; set; }

        [JsonPropertyName("change")]
        public double Change { get; set; }

        [JsonPropertyName("changePct")]
        public double ChangePct { get; set; }
    }
}
