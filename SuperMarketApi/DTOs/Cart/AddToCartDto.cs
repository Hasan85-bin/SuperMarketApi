using System.ComponentModel.DataAnnotations;

namespace SuperMarketApi.DTOs
{
    public record AddToCartDto
    {
        [Required, Range(1, int.MaxValue)]
        public int ProductID { get; init; }
        
        [Required, Range(1, 100)]
        public int Quantity { get; init; }
    }
} 