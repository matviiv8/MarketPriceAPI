using MarketPriceAPI.Application.Background;
using MarketPriceAPI.Application.Interfaces.Providers;
using MarketPriceAPI.Application.Interfaces.Services;
using MarketPriceAPI.Application.Profiles;
using MarketPriceAPI.Application.Services;
using MarketPriceAPI.Domain.Interfaces;
using MarketPriceAPI.Infrastructure.Context;
using MarketPriceAPI.Infrastructure.Providers;
using MarketPriceAPI.Infrastructure.Repositories;
using MarketPriceAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MarketPriceDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30)), mySqlConfig =>
        {
            mySqlConfig.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{ 
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Market Price API",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IMarketPriceService, MarketPriceService>();
builder.Services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));

builder.Services.AddScoped<IFintachartsTokenProvider, FintachartsTokenProvider>();
builder.Services.AddScoped<IFintachartsAssetProvider, FintachartsAssetProvider>();
builder.Services.AddScoped<ILivePriceProvider, LivePriceProvider>();
builder.Services.AddScoped<IHistoricalPriceProvider, HistoricalPriceProvider>();

builder.Services.AddScoped<IAssetRepository, AssetRepository>();

builder.Services.AddHostedService<AssetSyncBackgroundService>();

builder.Services.AddAutoMapper(typeof(AssetProfile));
builder.Services.AddAutoMapper(typeof(MarketPriceProfile));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
