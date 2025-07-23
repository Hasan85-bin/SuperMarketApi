using SuperMarketApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMarketApi.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> ExistByIdAsync(int id);
    }
} 