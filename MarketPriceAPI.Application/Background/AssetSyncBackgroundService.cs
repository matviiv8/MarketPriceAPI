using MarketPriceAPI.Application.Interfaces.Providers;
using MarketPriceAPI.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MarketPriceAPI.Application.Background
{
    public class AssetSyncBackgroundService : BackgroundService
    {
        private readonly ILogger<AssetSyncBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AssetSyncBackgroundService(ILogger<AssetSyncBackgroundService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var assetProvider = scope.ServiceProvider.GetRequiredService<IFintachartsAssetProvider>();
                        var assetRepository = scope.ServiceProvider.GetRequiredService<IAssetRepository>();
                        var allAssets = await assetRepository.GetAllAsync();
                        var assetsFromApi = await assetProvider.GetAssetsAsync();

                        if (!allAssets.Any())
                        {
                            _logger.LogInformation("Assets table is empty. Adding all {Count} assets from API.", assetsFromApi.Count());

                            await assetRepository.AddRangeAsync(assetsFromApi);
                            _logger.LogInformation("Assets loaded successfully.");
                        }
                        else
                        {
                            var existingIds = new HashSet<string>(allAssets.Select(asset => asset.Id));
                            var newAssets = assetsFromApi.Where(asset => !existingIds.Contains(asset.Id)).ToList();

                            if (newAssets.Any())
                            {
                                _logger.LogInformation("Adding {Count} new assets from API.", newAssets.Count);

                                await assetRepository.AddRangeAsync(newAssets);
                                _logger.LogInformation("New assets added successfully.");
                            }
                            else
                            {
                                _logger.LogInformation("No new assets to add. Assets table already up-to-date.");
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "An error occurred while loading assets.");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
