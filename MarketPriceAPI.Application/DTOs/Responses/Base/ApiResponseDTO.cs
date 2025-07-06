namespace MarketPriceAPI.Application.DTOs.Responses.Base
{
    public class ApiResponseDTO<T>
    {
        public PagingInfo Paging { get; set; }
        public List<T> Data { get; set; }
    }

    public class PagingInfo
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int Items { get; set; }
    }
}
