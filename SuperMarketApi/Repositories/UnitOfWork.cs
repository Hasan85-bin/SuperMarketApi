using System;
using System.Threading.Tasks;

namespace SuperMarketApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SuperMarketDbContext _context;
        public IProductRepository Products { get; }
        public IUserRepository Users { get; }
        public IPurchaseCartRepository PurchaseCarts { get; }

        public UnitOfWork(SuperMarketDbContext context, IProductRepository productRepo, IUserRepository userRepo, IPurchaseCartRepository purchaseCartRepo)
        {
            _context = context;
            Products = productRepo;
            Users = userRepo;
            PurchaseCarts = purchaseCartRepo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
} 