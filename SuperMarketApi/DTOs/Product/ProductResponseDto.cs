using SuperMarketApi.Models;

namespace SuperMarketApi.DTOs.Product
{
    public record ProductResponseDto
    {
        public int ID { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public string Brand { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public double Price { get; init; }
        public int Quantity { get; init; }
    }
} 