using SuperMarketApi.Models;

namespace SuperMarketApi.DTOs.Product
{
    public record ProductResponseDto(
        int ID,
        string Name,
        string Brand,
        string Category,
        double Price
    );
} 