using MarketPriceAPI.Application.DTOs.Requests.Assets;
using MarketPriceAPI.Application.DTOs.Responses.Assets;
using MarketPriceAPI.Application.DTOs.Responses.Base;

namespace MarketPriceAPI.Application.Interfaces.Services
{
    public interface IAssetService
    {
        Task<PaginatedResultResponseDTO<AssetResponseDTO>> GetAllAssetsAsync(AssetsRequestDTO filter);
    }
}
