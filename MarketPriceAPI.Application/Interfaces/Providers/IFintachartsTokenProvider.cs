namespace MarketPriceAPI.Application.Interfaces.Providers
{
    public interface IFintachartsTokenProvider
    {
        Task<string> GetAccessTokenAsync();
    }
}
