using SuperMarketApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMarketApi.Repositories
{
    public interface IPurchaseCartRepository
    {
        Task<IEnumerable<CartItem>> GetCartForUserAsync(int userId, bool includeProduct = false, bool includeUser = false);
        Task<CartItem?> GetItemAsync(int userId, int itemId);
        Task AddToCartAsync(CartItem item);
        void UpdateItem(CartItem item);
        void DeleteItem(CartItem item);
        Task Purchase(Purchase newPurchase);
        Task<IEnumerable<Purchase>> GetPurchaseHistoryAsync(int userId);
        Task<IEnumerable<Purchase>> GetDailyPurchaseHistory(DateOnly date);
        Task<Purchase?> GetPurchaseByIdAsync(int purchaseId);
        Task<Purchase?> GetLatestPendingRequest();
        void UpdatePurchase(Purchase purchase);
        Task<IEnumerable<Purchase>> GetPurchasesFilteredAsync(
            PurchaseStatus? status = null,
            int? userId = null,
            string? userName = null,
            DateTime? from = null,
            DateTime? to = null,
            bool includeUser = false
        );

    }
} 