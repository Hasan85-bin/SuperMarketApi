using SuperMarketApi.Models;
using SuperMarketApi.DTOs.Product;

namespace SuperMarketApi.Services
{
    public interface IProductService
    {
        IEnumerable<ProductResponseDto> GetAllProducts(
            string? search = null,
            string? category = null, 
            double? minPrice = null, 
            double? maxPrice = null
        );
        ProductResponseDto? GetProductById(int id);
        ProductResponseDto AddProduct(CreateProductDto newProduct);
        ProductResponseDto? UpdateProduct(int id, UpdateProductDto updatedProduct);
        bool DeleteProduct(int id);
    }
}
