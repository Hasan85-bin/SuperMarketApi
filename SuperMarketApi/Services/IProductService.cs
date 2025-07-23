using SuperMarketApi.Models;
using SuperMarketApi.DTOs.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMarketApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync(string? search = null, string? category = null, double? minPrice = null, double? maxPrice = null);
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<ProductResponseDto> AddProductAsync(CreateProductDto newProduct);
        Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductDto updatedProduct);
        Task<bool> DeleteProductAsync(int id);

    }
}
