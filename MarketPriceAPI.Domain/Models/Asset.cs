using System.Text.Json.Serialization;

namespace MarketPriceAPI.Domain.Models
{
    public class Asset : BaseEntity<string>
    {
        public override string Id { get; set; }
        public string Symbol { get; set; }
        public string Kind { get; set; }
        public string Description {  get; set; }
    }
}
