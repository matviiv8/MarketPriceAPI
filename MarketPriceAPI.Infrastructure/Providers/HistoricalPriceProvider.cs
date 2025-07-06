using AutoMapper;
using MarketPriceAPI.Application.DTOs.Requests.HistoricalPrices;
using MarketPriceAPI.Application.DTOs.Responses.Base;
using MarketPriceAPI.Application.DTOs.Responses.HistoricalPrices;
using MarketPriceAPI.Application.Interfaces.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MarketPriceAPI.Infrastructure.Providers
{
    public class HistoricalPriceProvider : IHistoricalPriceProvider
    {
        private readonly IFintachartsTokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<HistoricalPriceProvider> _logger;

        public HistoricalPriceProvider(
            IFintachartsTokenProvider tokenProvider,
            IHttpClientFactory httpClientFactory,
            IConfiguration config,
            IMapper mapper,
            ILogger<HistoricalPriceProvider> logger)
        {
            _tokenProvider = tokenProvider;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<HistoricalPriceResponseDTO>> GetHistoricalMarketPricesAsync(string assetId, HistoricalPriceRequestDTO requestDto)
        {
            _logger.LogInformation("Starting GetHistoricalMarketPricesAsync for assetId: {AssetId}", assetId);

            try
            {
                _logger.LogInformation("Requesting access token...");
                var accessToken = await _tokenProvider.GetAccessTokenAsync();
                _logger.LogInformation("Access token acquired.");

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var baseUrl = _config["FintachartsApi:RestUrl"];
                var url = $"{baseUrl}/api/bars/v1/bars/date-range?" +
                          $"instrumentId={assetId}" +
                          $"&provider=simulation" +
                          $"&interval={requestDto.Interval}" +
                          $"&periodicity={requestDto.Periodicity}" +
                          $"&startDate={requestDto.StartDate:yyyy-MM-dd}" +
                          $"&endDate={requestDto.EndDate:yyyy-MM-dd}";

                _logger.LogDebug("Constructed URL: {Url}", url);

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDTO<HistoricalPriceApiResponseDTO>>();

                if (!apiResponse.Data.Any())
                {
                    _logger.LogWarning("No data received from Fintacharts for assetId: {AssetId}", assetId);
                    return Enumerable.Empty<HistoricalPriceResponseDTO>();
                }

                var priceDtos = _mapper.Map<IEnumerable<HistoricalPriceResponseDTO>>(apiResponse.Data);
                _logger.LogInformation("Fetched {Count} historical price records for assetId: {AssetId}", priceDtos.Count(), assetId);

                return priceDtos;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unexpected error while fetching historical prices for assetId: {AssetId}", assetId);
                throw;
            }
        }
    }
}
