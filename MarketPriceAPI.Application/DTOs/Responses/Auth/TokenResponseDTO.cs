using System.Text.Json.Serialization;

namespace MarketPriceAPI.Application.DTOs.Responses.Auth
{
    public class TokenResponseDTO
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
