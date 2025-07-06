using AutoMapper;
using MarketPriceAPI.Application.DTOs.Requests.Assets;
using MarketPriceAPI.Application.DTOs.Responses.Assets;
using MarketPriceAPI.Application.DTOs.Responses.Base;
using MarketPriceAPI.Application.Interfaces.Services;
using MarketPriceAPI.Domain.Interfaces;
using MarketPriceAPI.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketPriceAPI.Application.Services
{
    public class AssetService : IAssetService
    {
        private readonly ILogger<AssetService> _logger;
        private readonly IAssetRepository _assetRepository;
        private readonly IPaginationService<Asset> _paginationService;
        private readonly IMapper _mapper;

        public AssetService(
            ILogger<AssetService> logger,
            IAssetRepository assetRepository,
            IPaginationService<Asset> paginationService,
            IMapper mapper)
        {
            _logger = logger;
            _assetRepository = assetRepository;
            _paginationService = paginationService;
            _mapper = mapper;
        }

        public async Task<PaginatedResultResponseDTO<AssetResponseDTO>> GetAllAssetsAsync(AssetsRequestDTO filter)
        {
            try
            {
                _logger.LogInformation("Start retrieving assets with filter: Symbol={Symbol}, Kind={Kind}, Page={Page}, PageSize={PageSize}",
                    filter.Symbol, filter.Kind, filter.Page, filter.PageSize);

                var allAssetsEntities = await _assetRepository.GetFilteredAllAsync(filter.Symbol, filter.Kind);
                _logger.LogInformation("Retrieved {Count} assets from repository", allAssetsEntities.Count());

                var paginatedAllAssets = await _paginationService.PaginateWithTotalPages(allAssetsEntities, filter.Page, filter.PageSize);
                _logger.LogInformation("Paginated assets: Page {Page} of {TotalPages}", filter.Page, paginatedAllAssets.TotalPages);

                var allAssetsDtos = _mapper.Map<List<AssetResponseDTO>>(paginatedAllAssets.Items);
                _logger.LogInformation("Mapped assets to DTOs. Returning result");

                var paginatedAssetsResult = new PaginatedResultResponseDTO<AssetResponseDTO>
                {
                    Items = allAssetsDtos,
                    TotalPages = paginatedAllAssets.TotalPages,
                    TotalItems = allAssetsEntities.Count()
                };

                return paginatedAssetsResult;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error retrieving all assets with filter: {FilterJson}", JsonSerializer.Serialize(filter));
                throw new ApplicationException($"Error retrieving all assets: {exception.Message}");
            }
        }
    }
}
