using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SuperMarketApi.DTOs.Product;
using SuperMarketApi.Models;
using SuperMarketApi.Repositories;

namespace SuperMarketApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync(string? search = null, string? category = null, double? minPrice = null, double? maxPrice = null)
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var query = products.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.ProductName.Contains(search) || p.Brand.Contains(search));
            }
            if (!string.IsNullOrEmpty(category))
            {
                if (Enum.TryParse<CategoryEnum>(category, ignoreCase: true, out var parsedCategory))
                    query = query.Where(p => p.Category == parsedCategory);
                else
                    throw new BadHttpRequestException("Invalid Category");
            }
            if (minPrice != null)
            {
                query = query.Where(p => p.Price >= minPrice);
            }
            if (maxPrice != null)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }
            return _mapper.Map<IEnumerable<ProductResponseDto>>(query);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return product != null ? _mapper.Map<ProductResponseDto>(product) : null;
        }

        public async Task<ProductResponseDto> AddProductAsync(CreateProductDto newProduct)
        {
            var product = _mapper.Map<Product>(newProduct);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductDto updatedProduct)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null)
                return null;
            _mapper.Map(updatedProduct, existingProduct);
            await _unitOfWork.Products.UpdateAsync(existingProduct);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductResponseDto>(existingProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var result = await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<bool> ProductExists(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            return product != null;
        }




    }
}