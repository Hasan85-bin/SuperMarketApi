using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SuperMarketApi.DTOs.Product;
using SuperMarketApi.Models;
using SuperMarketApi.Repositories;

namespace SuperMarketApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private int _nextId = 4;

        

        public ProductService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync(string? search = null, string? category = null, double? minPrice = null, double? maxPrice = null)
        {
            var products = await _productRepository.GetAllAsync();
            var query = products.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.name.Contains(search) || p.brand.Contains(search));
            }
            if (!string.IsNullOrEmpty(category))
            {
                if (Enum.TryParse<CategoryEnum>(category, ignoreCase: true, out var parsedCategory))
                    query = query.Where(p => p.category == parsedCategory);
                else
                    throw new BadHttpRequestException("Invalid Category");
            }
            if (minPrice != null)
            {
                query = query.Where(p => p.price >= minPrice);
            }
            if (maxPrice != null)
            {
                query = query.Where(p => p.price <= maxPrice);
            }
            return _mapper.Map<IEnumerable<ProductResponseDto>>(query);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null ? _mapper.Map<ProductResponseDto>(product) : null;
        }

        public async Task<ProductResponseDto> AddProductAsync(CreateProductDto newProduct)
        {
            var product = _mapper.Map<Product>(newProduct);
            await _productRepository.AddAsync(product);
            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductDto updatedProduct)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                return null;
            _mapper.Map(updatedProduct, existingProduct);
            await _productRepository.UpdateAsync(existingProduct);
            return _mapper.Map<ProductResponseDto>(existingProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;
            await _productRepository.DeleteAsync(id);
            return true;
        }




    }
}