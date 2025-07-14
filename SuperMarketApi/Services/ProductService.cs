using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SuperMarketApi.DTOs.Product;
using SuperMarketApi.Models;

namespace SuperMarketApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ICollection<Product> _products = new List<Product>();
        private readonly IMapper _mapper;
        private int _nextId = 4;

        

        public ProductService(IMapper mapper)
        {
            _products.Add(new Product { ID = 1, name = "Apple", brand = "FreshFarms", category = CategoryEnum.Fruits, price = 1.99 });
            _products.Add(new Product { ID = 2, name = "Milk", brand = "TropicalCo", category = CategoryEnum.Dairy, price = 0.99 });
            _products.Add(new Product { ID = 3, name = "Juice", brand = "BerryBest", category = CategoryEnum.Drinks, price = 2.99 });
            _mapper = mapper;
        }

        public IEnumerable<ProductResponseDto> GetAllProducts
        (string? search = null, string? category = null, double? minPrice = null, double? maxPrice = null){
            var products = _products.AsQueryable();
            if(!string.IsNullOrEmpty(search)){
                products = products.Where(p => p.name.Contains(search) || p.brand.Contains(search));
            }
            if(!string.IsNullOrEmpty(category)){
                if (Enum.TryParse<CategoryEnum>(category, ignoreCase: true, out var parsedCategory))
                    products = products.Where(p => p.category == parsedCategory);
                else
                    throw new BadHttpRequestException("Invalid Category");
            }
            if(minPrice != null){
                products = products.Where(p => p.price >= minPrice);
            }
            if(maxPrice != null){
                products = products.Where(p => p.price <= maxPrice);
            }
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public ProductResponseDto? GetProductById(int id){
            var product = _products.FirstOrDefault(p => p.ID == id);
            return product != null ? _mapper.Map<ProductResponseDto>(product) : null;
        }

        public ProductResponseDto AddProduct(CreateProductDto newProduct){
            var product = _mapper.Map<Product>(newProduct);
            product.ID = _nextId++;
            _products.Add(product);
            return _mapper.Map<ProductResponseDto>(product);
        }

        public ProductResponseDto? UpdateProduct(int id, UpdateProductDto updatedProduct){
            var existingProduct = _products.FirstOrDefault(p => p.ID == id);
            if (existingProduct == null)
                return null;
            _mapper.Map(updatedProduct, existingProduct);

            return _mapper.Map<ProductResponseDto>(existingProduct);
        }

        public bool DeleteProduct(int id){
            var product = _products.FirstOrDefault(p => p.ID == id);
            if (product == null)
                return false;
            _products.Remove(product);
            return true;
        }




    }
}