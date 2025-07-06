using AutoMapper;
using MarketPriceAPI.Application.DTOs.Requests.HistoricalPrices;
using MarketPriceAPI.Application.DTOs.Responses.HistoricalPrices;
using MarketPriceAPI.Application.DTOs.Responses.LivePrices;
using MarketPriceAPI.Application.Interfaces.Providers;
using MarketPriceAPI.Application.Interfaces.Services;
using MarketPriceAPI.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MarketPriceAPI.Application.Services
{
    public class MarketPriceService : IMarketPriceService
    {
        private readonly IHistoricalPriceProvider _historicalPriceProvider;
        private readonly ILivePriceProvider _realTimePriceProvider;
        private readonly IPaginationService<HistoricalPriceResponseDTO> _paginationService;
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MarketPriceService> _logger;

        public MarketPriceService(
            IHistoricalPriceProvider historicalPriceProvider,
            ILivePriceProvider realTimePriceProvider,
            IPaginationService<HistoricalPriceResponseDTO> paginationService,
            IAssetRepository assetRepository,
            IMapper mapper,
            ILogger<MarketPriceService> logger)
        {
            _historicalPriceProvider = historicalPriceProvider;
            _realTimePriceProvider = realTimePriceProvider;
            _paginationService = paginationService;
            _assetRepository = assetRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HistoricalPricePaginatedResponseDTO> GetAssetHistoricalPricesAsync(string assetId, HistoricalPriceRequestDTO requestDto)
        {
            try
            {
                _logger.LogInformation("Fetching historical prices for assetId: {AssetId}", assetId);

                var historicalPrices = await _historicalPriceProvider.GetHistoricalMarketPricesAsync(assetId, requestDto);
                _logger.LogInformation("Received {Count} price records for assetId: {AssetId}", historicalPrices.Count(), assetId);

                var paginatedPrices = await _paginationService.PaginateWithTotalPages(historicalPrices, 1, 50);
                _logger.LogInformation("Paginated to {TotalPages} pages", paginatedPrices.TotalPages);

                var asset = await _assetRepository.GetByIdAsync(assetId);
                var responseDto = new HistoricalPricePaginatedResponseDTO
                {
                    Items = paginatedPrices.Items.ToList(),
                    TotalPages = paginatedPrices.TotalPages,
                    TotalItems = paginatedPrices.Items.Count(),
                    Symbol = asset.Symbol
                };

                _logger.LogInformation("Returning historical prices for asset symbol: {Symbol}", asset.Symbol);
                return responseDto;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to fetch historical prices for assetId: {AssetId}", assetId);
                throw;
            }
        }

        public async Task<LastLivePriceResponseDTO> GetLastLivePriceAsync(string assetId)
        {
            try
            {
                _logger.LogInformation("Fetching last live price for assetId: {AssetId}", assetId);

                var lastLivePrive = await _realTimePriceProvider.GetLiveMarketPriceAsync(assetId);
                _logger.LogInformation("Received last live price records for assetId: {AssetId}", assetId);

                var asset = await _assetRepository.GetByIdAsync(assetId);
                var responseDto = _mapper.Map<LastLivePriceResponseDTO>(lastLivePrive);
                responseDto.Symbol = asset.Symbol;

                _logger.LogInformation("Returning historical prices for asset symbol: {Symbol}", asset.Symbol);
                return responseDto;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to fetch last live price for assetId: {AssetId}", assetId);
                throw;
            }
        }
    }
}
