using System.ComponentModel.DataAnnotations;
using SuperMarketApi.Models;

namespace SuperMarketApi.DTOs.Product
{
    public record UpdateProductDto(
        [Required, MaxLength(50), MinLength(3)] string Name,
        [Required, MaxLength(50), MinLength(3)] string Brand,
        [Required, EnumDataType(typeof(CategoryEnum))] CategoryEnum Category,
        [Required, Range(0, double.MaxValue)] double Price
    );
} 