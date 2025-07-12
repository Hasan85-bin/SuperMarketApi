using SuperMarketApi.Models;
using SuperMarketApi.DTOs;

namespace SuperMarketApi.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts(
            string? category = null, 
            double? minPrice = null, 
            double? maxPrice = null
        );
        Product? GetProductById(int id);
        Product AddProduct(CreateProductDto newProduct);
        Product? UpdateProduct(int id, UpdateProductDto updatedProduct);
        bool DeleteProduct(int id);
    }
}
