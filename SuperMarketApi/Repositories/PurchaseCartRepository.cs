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
            query = query.Include(c => c.Product);
        }
        if (includeUser)
        {
            query = query.Include(c => c.User);
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

    public void UpdateItem(CartItem item)
    {
        _context.CartItems.Update(item);
    }

    public void DeleteItem(CartItem item)
    {
        _context.Remove(item);
    }

    public async Task Purchase(Purchase newPurchase)
    {
        await _context.Purchases.AddAsync(newPurchase);
    }



    public async Task<Purchase?> GetLatestPendingRequest()
    {
        return await _context.Purchases.Include(p => p.Items).FirstOrDefaultAsync(p => p.Status == PurchaseStatus.Pending);
    }

    public async Task<Purchase?> GetPurchaseByIdAsync(int purchaseId, bool includeUser, bool includeItems)
    {
        var query = _context.Purchases.AsQueryable();
        if (includeItems)
            query = query.Include(p => p.Items);
        if (includeUser)
            query = query.Include(p => p.User);
        return await query.FirstOrDefaultAsync(p => p.ID == purchaseId);
    }

    public void UpdatePurchase(Purchase purchase)
    {
        _context.Purchases.Update(purchase);
    }

    public async Task<IEnumerable<Purchase>> GetPurchasesFilteredAsync(
        PurchaseStatus? status = null,
        int? userId = null,
        string? userName = null,
        DateTime? from = null,
        DateTime? to = null,
        bool includeUser = false,
        bool includeItems = false
    )
    {
        var query = _context.Purchases.AsQueryable();
        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);
        if (userId.HasValue)
            query = query.Where(p => p.UserID == userId.Value);
        if (!string.IsNullOrEmpty(userName))
            query = query.Where(p => p.User != null && p.User.UserName == userName);
        if (from.HasValue)
            query = query.Where(p => p.PurchaseDate >= from.Value);
        if (to.HasValue)
            query = query.Where(p => p.PurchaseDate <= to.Value);
        if (includeUser)
            query = query.Include(p => p.User);
        if (includeItems)
            query = query.Include(p => p.Items);
        return await query.ToListAsync();
    }


}
