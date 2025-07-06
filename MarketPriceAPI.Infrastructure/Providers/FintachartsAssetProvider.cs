using MarketPriceAPI.Application.DTOs.Responses.Base;
using MarketPriceAPI.Application.Interfaces.Providers;
using MarketPriceAPI.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MarketPriceAPI.Infrastructure.Providers
{
    public class FintachartsAssetProvider : IFintachartsAssetProvider
    {
        private readonly IFintachartsTokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<FintachartsAssetProvider> _logger;

        public FintachartsAssetProvider(
            IFintachartsTokenProvider tokenProvider,
            IHttpClientFactory httpClientFactory, 
            IConfiguration config,
            ILogger<FintachartsAssetProvider> logger)
        {
            _tokenProvider = tokenProvider;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _logger = logger;
        }

        public async Task<IEnumerable<Asset>> GetAssetsAsync()
        {
            _logger.LogInformation("Starting GetAssetsAsync...");
            var allAssets = new List<Asset>();
            int currentPage = 1;
            int totalPages = 1;

            try
            {
                _logger.LogInformation("Requesting access token...");
                var accessToken = await _tokenProvider.GetAccessTokenAsync();
                _logger.LogInformation("Access token acquired.");

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var baseUrl = _config["FintachartsApi:RestUrl"];

                while (currentPage <= totalPages)
                {
                    var url = $"{baseUrl}/api/instruments/v1/instruments?providers=simulation&page={currentPage}&pageSize=100";
                    _logger.LogDebug("Requesting assets from: {Url}", url);

                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Failed to fetch assets on page {Page}: {StatusCode}", currentPage, response.StatusCode);
                        break;
                    }

                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDTO<Asset>>();

                    if (apiResponse?.Data == null)
                    {
                        _logger.LogWarning("No data received from API on page {Page}", currentPage);
                        break;
                    }

                    allAssets.AddRange(apiResponse.Data);

                    totalPages = apiResponse.Paging?.Pages ?? 1;
                    currentPage++;
                }

                _logger.LogInformation("Successfully fetched {Count} assets from Fintacharts API.", allAssets.Count);
                return allAssets;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unexpected error occurred while fetching assets.");
                throw;
            }
        }
    }
}
