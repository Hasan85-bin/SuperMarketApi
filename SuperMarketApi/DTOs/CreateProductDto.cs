using System.ComponentModel.DataAnnotations;
using SuperMarketApi.Models;

namespace SuperMarketApi.DTOs
{
    public record CreateProductDto(
        [property: Required, MaxLength(50), MinLength(3)] string Name,
        [property: Required, MaxLength(50), MinLength(3)] string Brand,
        [property: Required, EnumDataType(typeof(Category))] Category Category,
        [property: Required, Range(0, double.MaxValue)] double Price
    );
}