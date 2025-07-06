using MarketPriceAPI.Application.DTOs.Responses.LivePrices;
using MarketPriceAPI.Application.Interfaces.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace MarketPriceAPI.Infrastructure.Providers
{
    public class LivePriceProvider : ILivePriceProvider
    {
        private readonly IFintachartsTokenProvider _tokenProvider;
        private readonly ClientWebSocket _clientWebSocket;
        private readonly IConfiguration _config;
        private readonly ILogger<LivePriceProvider> _logger;

        public LivePriceProvider(
            IFintachartsTokenProvider tokenProvider,
            IConfiguration config,
            ILogger<LivePriceProvider> logger)
        {
            _config = config;
            _tokenProvider = tokenProvider;
            _clientWebSocket = new ClientWebSocket();
            _logger = logger;
        }

        public async Task<LivePriceApiResponseDTO> GetLiveMarketPriceAsync(string assetId)
        {
            _logger.LogInformation("Starting GetLiveMarketPriceAsync for assetId: {AssetId}", assetId);
            int maxReceiveIterations = 10;

            try
            {
                _logger.LogInformation("Requesting access token...");
                var accessToken = await _tokenProvider.GetAccessTokenAsync();
                _logger.LogInformation("Access token acquired.");

                var baseUrl = _config["FintachartsApi:WssUrl"];
                var uri = new Uri($"{baseUrl}/api/streaming/ws/v1/realtime?token={accessToken}");
                _logger.LogInformation("Connecting to WebSocket at {Uri}", uri);

                await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);
                _logger.LogInformation("WebSocket connected.");

                var subscription = new
                {
                    type = "l1-subscription",
                    id = Guid.NewGuid().ToString(),
                    instrumentId = assetId,
                    provider = "simulation",
                    subscribe = true,
                    kinds = new[] { "last" }
                };

                var subscriptionJson = JsonSerializer.Serialize(subscription);
                var subscriptionBytes = Encoding.UTF8.GetBytes(subscriptionJson);

                _logger.LogInformation("Sending subscription message.");
                await _clientWebSocket.SendAsync(new ArraySegment<byte>(subscriptionBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                _logger.LogInformation("Subscription message sent.");

                var livePrice = await ReceiveLivePricesAsync(maxReceiveIterations);
                _logger.LogInformation("Received live price data. Returning result.");

                return livePrice;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unexpected error while fetching live price for assetId: {AssetId}", assetId);
                throw;
            }
            finally
            {
                if (_clientWebSocket.State == WebSocketState.Open || _clientWebSocket.State == WebSocketState.CloseReceived)
                {
                    _logger.LogInformation("Closing WebSocket connection.");
                    await _clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing WebSocket connection", CancellationToken.None);
                    _logger.LogInformation("WebSocket connection closed.");
                }
            }
        }

        private async Task<LivePriceApiResponseDTO> ReceiveLivePricesAsync(int maxIterations)
        {
            var buffer = new byte[4096];
            var messageBuilder = new StringBuilder();
            LivePriceApiResponseDTO livePrice = null;

            int iteration = 0;

            while (iteration < maxIterations)
            {
                var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if (result.EndOfMessage)
                {
                    var fullMessage = messageBuilder.ToString();
                    messageBuilder.Clear();

                    livePrice = ParseLivePrice(fullMessage);
                    if (livePrice != null)
                    {
                        _logger.LogInformation("Received live price: {Timestamp} | {Price} | {Volume} | {Change} | {ChangePct}",
                            livePrice.Timestamp, livePrice.Price, livePrice.Volume, livePrice.Change, livePrice.ChangePct);
                    }
                    else
                    {
                        _logger.LogWarning("Received message did not contain 'last' price data.");
                    }
                }

                iteration++;
            }

            return livePrice;
        }

        private LivePriceApiResponseDTO ParseLivePrice(string json)
        {
            using var document = JsonDocument.Parse(json);

            if (document.RootElement.TryGetProperty("last", out var lastElement))
            {
                var lastJson = lastElement.GetRawText();
                return JsonSerializer.Deserialize<LivePriceApiResponseDTO>(lastJson);
            }

            return null;
        }
    }
}