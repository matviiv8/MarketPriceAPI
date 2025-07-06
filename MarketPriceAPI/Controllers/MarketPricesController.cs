using MarketPriceAPI.Application.DTOs.Requests.HistoricalPrices;
using MarketPriceAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MarketPriceAPI.Controllers
{
    [ApiController]
    [Route("api/market-prices")]
    public class MarketPricesController : Controller
    {
        private readonly ILogger<MarketPricesController> _logger;
        private readonly IMarketPriceService _marketPriceService;

        public MarketPricesController(IMarketPriceService marketPriceService, ILogger<MarketPricesController> logger)
        {
            _marketPriceService = marketPriceService;
            _logger = logger;
        }

        /// <summary>
        /// Get historical prices for a specific asset.
        /// </summary>
        /// <param name="assetId">The unique identifier of the asset.</param>
        /// <param name="requestDto">An object containing filtering parameters such as date range, resolution, etc.</param>
        /// <response code="200">Successful request. Returns a list of historical prices for the specified asset.</response>
        /// <response code="404">No historical price data found for the given parameters.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve historical price data.</response>
        [HttpGet("{assetId}/historical")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHistoricalPrices([FromRoute] string assetId, [FromQuery] HistoricalPriceRequestDTO requestDto)
        {
            var historicalPrices = await _marketPriceService.GetAssetHistoricalPricesAsync(assetId, requestDto);

            if (!historicalPrices.Items.Any())
            {
                _logger.LogWarning("No historical market prices found for request: {RequestDto}", JsonSerializer.Serialize(requestDto));
                return NotFound("No historical price data found for the given parameters.");
            }

            _logger.LogInformation("Successfully retrieved historical prices for request: {RequestDto}", JsonSerializer.Serialize(requestDto));
            return Ok(historicalPrices);
        }

        /// <summary>
        /// Get the last live price for a specific asset.
        /// </summary>
        /// <param name="assetId">The unique identifier of the asset.</param>
        /// <response code="200">Successful request. Returns the latest live price for the specified asset.</response>
        /// <response code="404">No live price data found for the given asset.</response>
        /// <response code="500">An internal server error occurred while trying to retrieve the live price data.</response>
        [HttpGet("{assetId}/live")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLastLivePrice([FromRoute] string assetId)
        {
            var lastLivePrice = await _marketPriceService.GetLastLivePriceAsync(assetId);

            if (lastLivePrice == null)
            {
                _logger.LogWarning("No live market price found for assetId: {AssetId}", assetId);
                return NotFound("No live price data found for the given asset.");
            }

            _logger.LogInformation("Successfully retrieved live price for assetId: {AssetId}", assetId);
            return Ok(lastLivePrice);
        }
    }
}
