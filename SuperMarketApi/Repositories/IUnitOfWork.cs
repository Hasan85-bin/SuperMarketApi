using System;
using System.Threading.Tasks;

namespace SuperMarketApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IUserRepository Users { get; }
        IPurchaseCartRepository PurchaseCarts { get; }
        Task<int> SaveChangesAsync();
    }
} 