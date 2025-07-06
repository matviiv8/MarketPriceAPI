using AutoMapper;
using MarketPriceAPI.Application.DTOs.Responses.HistoricalPrices;
using MarketPriceAPI.Application.DTOs.Responses.LivePrices;

namespace MarketPriceAPI.Application.Profiles
{
    public class MarketPriceProfile : Profile
    {
        public MarketPriceProfile()
        {
            CreateMap<HistoricalPriceApiResponseDTO, HistoricalPriceResponseDTO>();
            CreateMap<LivePriceApiResponseDTO, LastLivePriceResponseDTO>();
        }
    }
}
