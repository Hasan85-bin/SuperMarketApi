using SuperMarketApi.DTOs.Cart;
using SuperMarketApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMarketApi.Services
{
    public interface ICartPurchaseService
    {
        Task<IEnumerable<CartItem>> GetCartForUserAsync(int userId);
        Task<CartItem> GetItemAsync(int userId, int itemId);
        Task AddToCartAsync(int userId, CartItemCreateDto dto);
        Task UpdateItemAsync(int userId, int itemId, CartItemUpdateDto dto);
        Task DeleteItemAsync(int userId, int itemId);
        Task Purchase(int userId);
        Task<IEnumerable<Purchase>> GetPurchaseHistoryForUser(int userId);
        Task<IEnumerable<Purchase>> GetCompletePurchaseHistory();
    }
} 