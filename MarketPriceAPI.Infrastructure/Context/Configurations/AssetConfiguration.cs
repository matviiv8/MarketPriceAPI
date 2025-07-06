using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MarketPriceAPI.Domain.Models;

namespace MarketPriceAPI.Infrastructure.Context.Configurations
{
    internal class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(asset => asset.Id);
            
            builder.Property(asset => asset.Id).IsRequired().HasMaxLength(100);
            builder.Property(asset => asset.Symbol).IsRequired().HasMaxLength(20);
            builder.Property(asset => asset.Kind).IsRequired().HasMaxLength(50);
            builder.Property(asset => asset.Description).IsRequired().HasMaxLength(100);
        }
    }
}
