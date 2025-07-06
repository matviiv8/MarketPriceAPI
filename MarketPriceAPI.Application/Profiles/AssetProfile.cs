using AutoMapper;
using MarketPriceAPI.Application.DTOs.Responses.Assets;
using MarketPriceAPI.Domain.Models;

namespace MarketPriceAPI.Application.Profiles
{
    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            CreateMap<Asset, AssetResponseDTO>();
        }
    }
}
