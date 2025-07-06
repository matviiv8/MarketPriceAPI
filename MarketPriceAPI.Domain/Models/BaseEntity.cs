namespace MarketPriceAPI.Domain.Models
{
    public abstract class BaseEntity<TKey>
    {
        public abstract TKey Id { get; set; }
    }
}
