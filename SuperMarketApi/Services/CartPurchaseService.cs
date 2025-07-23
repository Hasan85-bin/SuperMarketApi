using SuperMarketApi.DTOs.Cart;
using SuperMarketApi.Models;
using SuperMarketApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System;
using System.Linq;

namespace SuperMarketApi.Services
{
    public class CartPurchaseService : ICartPurchaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartPurchaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        public async Task DeleteItemAsync(int userId, int itemId)
        {
            var item = await _unitOfWork.PurchaseCarts.GetItemAsync(userId, itemId);
            if (item == null)
                throw new BadHttpRequestException("Item NotFound");
            await _unitOfWork.PurchaseCarts.DeleteItemAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartForUserAsync(int userId)
        {
            return await _unitOfWork.PurchaseCarts.GetCartForUserAsync(userId, true);
        }

        public async Task<CartItem> GetItemAsync(int userId, int itemId)
        {
            if (!await _unitOfWork.Users.ExistByIdAsync(userId))
                throw new BadHttpRequestException("User NotFound");
            var item = await _unitOfWork.PurchaseCarts.GetItemAsync(userId, itemId);
            if (item == null)
                throw new BadHttpRequestException("Item NotFound");
            return item;
        }

        public async Task UpdateItemAsync(int userId ,int itemId, CartItemUpdateDto dto)
        {
            var item = await GetItemAsync(userId, itemId);
            _mapper.Map(dto, item);
            await _unitOfWork.PurchaseCarts.UpdateItemAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddToCartAsync(int userId, CartItemCreateDto dto)
        {
            if (!await _unitOfWork.Users.ExistByIdAsync(userId))
                throw new BadHttpRequestException("User NotFound");
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductID);
            if (product == null)
                throw new BadHttpRequestException("Product NotFound");
            if (dto.Quantity > product.Quantity)
                throw new BadHttpRequestException($"Not enough stock for product {product.ProductName}. Available: {product.Quantity}");
            var item = _mapper.Map<CartItem>(dto);
            item.UserID = userId;
            await _unitOfWork.PurchaseCarts.AddToCartAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Purchase(int userId)
        {
            var cart = await _unitOfWork.PurchaseCarts.GetCartForUserAsync(userId, true);
            foreach (var cartItem in cart)
            {
                if (cartItem.Product == null)
                    throw new BadHttpRequestException($"Product with ID {cartItem.ProductID} not found.");
                if (cartItem.Quantity > cartItem.Product.Quantity)
                    throw new BadHttpRequestException($"Not enough stock for product {cartItem.Product.ProductName}. Available: {cartItem.Product.Quantity}");
            }
            var newPurchases = _mapper.Map<List<Purchase>>(cart);
            foreach (var purchase in newPurchases)
            {
                purchase.TotalPrice = purchase.Price * purchase.Quantity;
                purchase.PurchaseDate = DateTime.UtcNow;
            }
            await _unitOfWork.PurchaseCarts.Purchase(newPurchases);
            foreach (var item in cart)
            {
                item.Product!.Quantity -= item.Quantity;
                await _unitOfWork.Products.UpdateAsync(item.Product);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Purchase>> GetPurchaseHistoryForUser(int userId)
        {
            return await _unitOfWork.PurchaseCarts.GetPurchaseHistoryAsync(userId);
        }


        public async Task<IEnumerable<Purchase>> GetCompletePurchaseHistory()
        {
            return await _unitOfWork.PurchaseCarts.GetCompletePurchaseHistory();
        }







    }
} 