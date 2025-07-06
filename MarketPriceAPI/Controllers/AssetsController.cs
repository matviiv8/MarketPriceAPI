using MarketPriceAPI.Application.DTOs.Requests.Assets;
using MarketPriceAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MarketPriceAPI.Controllers
{
    [ApiController]
    [Route("api/assets")]
    public class AssetsController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetService assetService, ILogger<AssetsController> logger)
        {
            _assetService = assetService;
            _logger = logger;
        }

        /// <summary>
        /// Get list of assets.
        /// </summary>
        /// <param name="filter">Filter object to filter the list of assets.</param>
        /// <response code="200">Successful request. Returns a list of assets with the total number of pages and the number of objects.</response>
        /// <response code="404">No assets found according to these parameters.</response>
        /// <response code="500">An internal server error occurred while trying to get the list of assets.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAssets([FromQuery] AssetsRequestDTO filter)
        {
            var assets = await _assetService.GetAllAssetsAsync(filter);

            if (!assets.Items.Any())
            {
                _logger.LogWarning("Assets not found for filter {FilterJson}", JsonSerializer.Serialize(filter));
                return StatusCode(StatusCodes.Status404NotFound);
            }

            _logger.LogInformation("Found {Count} assets for filter {FilterJson}", assets.Items.Count(), JsonSerializer.Serialize(filter));
            return StatusCode(StatusCodes.Status200OK, assets);
        }
    }
}
