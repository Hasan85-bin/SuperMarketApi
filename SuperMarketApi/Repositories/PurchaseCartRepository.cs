using Microsoft.EntityFrameworkCore;
using SuperMarketApi;
using SuperMarketApi.Models;
using SuperMarketApi.Repositories;
using System.Collections.Immutable;

public class PurchaseCartRepository : IPurchaseCartRepository
{
    private readonly SuperMarketDbContext _context;
    public PurchaseCartRepository(SuperMarketDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CartItem>> GetCartForUserAsync(int userId, bool includeProduct = false, bool includeUser = false)
    {
        var query = _context.CartItems.Where(c => c.UserID == userId);
        if (includeProduct)
        {
            query.Include(c => c.Product);
        }
        if (includeUser)
        {
            query.Include(c => c.User);
        }
        return await query.ToListAsync();
    }

    

    public async Task<CartItem?> GetItemAsync(int userId, int itemId)
    {
        return await _context.CartItems.FirstOrDefaultAsync(c => c.UserID == userId && c.ID == itemId);
    }
    public async Task AddToCartAsync(CartItem item)
    {
        await _context.CartItems.AddAsync(item);
    }

    public async Task UpdateItemAsync(CartItem item)
    {
        _context.CartItems.Update(item);
    }

    public async Task DeleteItemAsync(CartItem item)
    {
        _context.Remove(item);
    }

    public async Task Purchase(IEnumerable<Purchase> newPurchases)
    {
        await _context.Purchases.AddRangeAsync(newPurchases);
    }

    public async Task<IEnumerable<Purchase>> GetPurchaseHistoryAsync(int userId)
    {
        return await _context.Purchases.Where(p => p.UserID == userId).ToListAsync();
    }

    public async Task<IEnumerable<Purchase>> GetCompletePurchaseHistory()
    {
        return await _context.Purchases.ToListAsync();
    }
}
