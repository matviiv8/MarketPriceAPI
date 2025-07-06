using MarketPriceAPI.Application.DTOs.Responses.Auth;
using MarketPriceAPI.Application.Interfaces.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace MarketPriceAPI.Infrastructure.Providers
{
    public class FintachartsTokenProvider : IFintachartsTokenProvider
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<FintachartsTokenProvider> _logger;

        public FintachartsTokenProvider(
            IMemoryCache cache,
            IHttpClientFactory httpClientFactory,
            IConfiguration config,
            ILogger<FintachartsTokenProvider> logger)
        {
            _cache = cache;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _logger = logger;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            _logger.LogInformation("Starting GetAccessTokenAsync...");

            try
            {
                var cacheKey = _config["Cache:TokenCacheKey"];
                if (!string.IsNullOrEmpty(cacheKey) && _cache.TryGetValue(cacheKey, out string token))
                {
                    _logger.LogInformation("Access token successfully retrieved from cache.");
                    return token;
                }

                var clientId = _config["FintachartsApi:ClientId"];
                var grantType = _config["FintachartsApi:GrantType"];
                var baseUrl = _config["FintachartsApi:RestUrl"];
                var username = _config["FintachartsApi:Username"];
                var password = _config["FintachartsApi:Password"];

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(grantType) ||
                    string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    _logger.LogWarning("Missing one or more required configuration values.");
                    throw new InvalidOperationException("Incomplete configuration for access token request.");
                }

                var client = _httpClientFactory.CreateClient();
                var url = $"{baseUrl}/identity/realms/fintatech/protocol/openid-connect/token";

                _logger.LogInformation("Sending request to get new access token at {Url}...", url);

                var response = await client.PostAsync(url, new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", grantType },
                    { "client_id", clientId },
                    { "username", username },
                    { "password", password }
                }));

                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDTO>();

                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    _logger.LogError("Token response is null or missing access token.");
                    throw new InvalidOperationException("Invalid token response.");
                }

                _cache.Set(cacheKey, tokenResponse.AccessToken, TimeSpan.FromMinutes(29));
                _logger.LogInformation("Successfully fetched and cached new access token.");

                return tokenResponse.AccessToken;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching the access token.");
                throw;
            }
        }
    }
}
