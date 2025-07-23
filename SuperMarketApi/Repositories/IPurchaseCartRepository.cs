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
        Task UpdateItemAsync(CartItem item);
        Task DeleteItemAsync(CartItem item);
        Task Purchase(IEnumerable<Purchase> newPurchases);
        Task<IEnumerable<Purchase>> GetPurchaseHistoryAsync(int userId);
        Task<IEnumerable<Purchase>> GetCompletePurchaseHistory();
    }
} 