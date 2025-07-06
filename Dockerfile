FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY *.sln ./
COPY MarketPriceAPI/*.csproj MarketPriceAPI/
COPY MarketPriceAPI.Application/*.csproj MarketPriceAPI.Application/
COPY MarketPriceAPI.Infrastructure/*.csproj MarketPriceAPI.Infrastructure/
COPY MarketPriceAPI.Domain/*.csproj MarketPriceAPI.Domain/
RUN dotnet restore

COPY . .
WORKDIR /src/MarketPriceAPI
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MarketPriceAPI.dll"]
