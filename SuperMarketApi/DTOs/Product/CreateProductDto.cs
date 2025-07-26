using System.ComponentModel.DataAnnotations;
using SuperMarketApi.Models;
using static SuperMarketApi.Models.Product;

namespace SuperMarketApi.DTOs.Product
{
    public record CreateProductDto
    {
        [Required, MaxLength(50), MinLength(3)]
        public string Name { get; init; } = string.Empty;
        
        [Required, MaxLength(50), MinLength(3)]
        public string Brand { get; init; } = string.Empty;
        
        public CategoryEnum Category { get; init; }
        
        [Required, Range(0, double.MaxValue)]
        public double Price { get; init; }
        
        [Required, Range(0, int.MaxValue)]
        public int Quantity { get; init; }
    }
}