using AutoMapper;
using SuperMarketApi.DTOs;
using SuperMarketApi.Models;

namespace SuperMarketApi.Services
{
    public class ProductService
    {
        private readonly ICollection<Product> _products = new List<Product>();
        private readonly IMapper _mapper;
        private int _nextId = 4;

        

        public ProductService(IMapper mapper)
        {
            _products.Add(new Product { ID = 1, name = "Apple", brand = "FreshFarms", category = Category.Fruits, price = 1.99 });
            _products.Add(new Product { ID = 2, name = "Milk", brand = "TropicalCo", category = Category.Dairy, price = 0.99 });
            _products.Add(new Product { ID = 3, name = "Juice", brand = "BerryBest", category = Category.Drinks, price = 2.99 });
            _mapper = mapper;
        }

        public IEnumerable<Product> GetAllProducts
        (string? search = null, string? category = null, double? minPrice = null, double? maxPrice = null){
            var products = _products.AsQueryable();
            if(!string.IsNullOrEmpty(search)){
                products = products.Where(p => p.name.Contains(search) || p.brand.Contains(search));
            }
            if(!string.IsNullOrEmpty(category)){
                products = products.Where(p => p.category.ToString() == category);
            }
            if(minPrice != null){
                products = products.Where(p => p.price >= minPrice);
            }
            if(maxPrice != null){
                products = products.Where(p => p.price <= maxPrice);
            }
            return products;
        }

        public Product? GetProductById(int id){
            return _products.FirstOrDefault(p => p.ID == id);
        }

        public Product AddProduct(CreateProductDto newProduct){
            var product = _mapper.Map<Product>(newProduct);
            product.ID = _nextId++;
            _products.Add(product);
            return product;
        }

        public Product? UpdateProduct(int id, UpdateProductDto updatedProduct){
            var existingProduct = _products.FirstOrDefault(p => p.ID == id);
            if (existingProduct == null)
                return null;
            _mapper.Map(updatedProduct, existingProduct);

            return existingProduct;
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