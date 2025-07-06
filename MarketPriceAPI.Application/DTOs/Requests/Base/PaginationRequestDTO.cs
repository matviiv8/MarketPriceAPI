using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace MarketPriceAPI.Application.DTOs.Requests.Base
{
    public class PaginationRequestDTO
    {
        [FromQuery(Name = "page")]
        [DefaultValue(1)]
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        [DefaultValue(50)]
        [Range(1, 50, ErrorMessage = "Page size must be between 1 and 50.")]
        public virtual int PageSize { get; set; } = 6;
    }
}
