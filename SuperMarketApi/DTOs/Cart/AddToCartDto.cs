using System.ComponentModel.DataAnnotations;

namespace SuperMarketApi.DTOs
{
    public record AddToCartDto(
        [Required, Range(1, int.MaxValue)] int ProductID,
        [Required, Range(1, 100)] int Quantity
    );
} 