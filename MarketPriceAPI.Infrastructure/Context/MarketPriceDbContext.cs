using MarketPriceAPI.Domain.Models;
using MarketPriceAPI.Infrastructure.Context.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceAPI.Infrastructure.Context
{
    public class MarketPriceDbContext : DbContext
    {
        public MarketPriceDbContext(DbContextOptions<MarketPriceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Asset> Assets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AssetConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
