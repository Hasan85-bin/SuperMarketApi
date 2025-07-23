using System.ComponentModel.DataAnnotations;
using SuperMarketApi.Models;
using static SuperMarketApi.Models.Product;

namespace SuperMarketApi.DTOs.Product
{
    public record CreateProductDto(
        [Required, MaxLength(50), MinLength(3)] string Name,
        [Required, MaxLength(50), MinLength(3)] string Brand,
        CategoryEnum Category,
        [Required, Range(0, double.MaxValue)] double Price,
        [Required, Range(0, int.MaxValue)] int Quantity
    );
}