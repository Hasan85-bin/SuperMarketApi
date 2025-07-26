using SuperMarketApi.Models;

namespace SuperMarketApi.DTOs.Cart
{
    public record CartItemResponseDto
    {
        public int Quantity { get; init; }
        public ProductInfoDto Product { get; init; } = new();
    }

    public record ProductInfoDto
    {
        public int ID { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public string Brand { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public double Price { get; init; }
    }
} 