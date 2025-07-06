namespace MarketPriceAPI.Application.DTOs.Responses.Base
{
    public class PaginatedResultResponseDTO<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
