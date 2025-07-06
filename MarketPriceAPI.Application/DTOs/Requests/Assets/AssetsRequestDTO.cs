using MarketPriceAPI.Application.DTOs.Requests.Base;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace MarketPriceAPI.Application.DTOs.Requests.Assets
{
    public class AssetsRequestDTO : PaginationRequestDTO
    {
        [FromQuery(Name = "kind")]
        public string? Kind { get; set; }

        [FromQuery(Name = "symbol")]
        [DefaultValue("EUR/USD")]
        public string? Symbol { get; set; }
    }
}
