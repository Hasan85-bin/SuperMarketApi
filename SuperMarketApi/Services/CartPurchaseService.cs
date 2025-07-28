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
            if (!await _unitOfWork.Users.ExistByIdAsync(userId))
                throw new UnauthorizedAccessException("UserId NotValid");
            var item = await _unitOfWork.PurchaseCarts.GetItemAsync(userId, itemId);
            if (item == null)
                throw new BadHttpRequestException("Item NotFound");
            _unitOfWork.PurchaseCarts.DeleteItem(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItemResponseDto>> GetCartForUserAsync(int userId)
        {
            var cartItems = await _unitOfWork.PurchaseCarts.GetCartForUserAsync(userId, true);
            return _mapper.Map<IEnumerable<CartItemResponseDto>>(cartItems);
        }

        public async Task<CartItemResponseDto> GetItemAsync(int userId, int itemId)
        {
            if (!await _unitOfWork.Users.ExistByIdAsync(userId))
                throw new BadHttpRequestException("User NotFound");
            var item = await _unitOfWork.PurchaseCarts.GetItemAsync(userId, itemId);
            if (item == null)
                throw new BadHttpRequestException("Item NotFound");
            return _mapper.Map<CartItemResponseDto>(item);
        }

        public async Task UpdateItemAsync(int userId ,int itemId, CartItemUpdateDto dto)
        {
            if (!await _unitOfWork.Users.ExistByIdAsync(userId))
                throw new UnauthorizedAccessException("UserId NotValid");
            var item = await _unitOfWork.PurchaseCarts.GetItemAsync(userId, itemId);
            if (item == null)
                throw new BadHttpRequestException("Item NotFound");
            _mapper.Map(dto, item);
            _unitOfWork.PurchaseCarts.UpdateItem(item);
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

        public async Task Purchase(int userId, string postCode)
        {
            var cart = await _unitOfWork.PurchaseCarts.GetCartForUserAsync(userId, true);
            foreach (var cartItem in cart)
            {
                if (cartItem.Product == null)
                    throw new BadHttpRequestException($"Product with ID {cartItem.ProductID} not found.");
                if (cartItem.Quantity > cartItem.Product.Quantity)
                    throw new BadHttpRequestException($"Not enough stock for product {cartItem.Product.ProductName}. Available: {cartItem.Product.Quantity}");
            }
            var newPurchase = new Purchase();
            newPurchase.UserID = userId;
            newPurchase.PostCode = postCode;
            newPurchase.PurchaseDate = DateTime.UtcNow;
            
            // Map cart items to purchase items
            var purchaseItems = _mapper.Map<List<PurchaseItem>>(cart);
            newPurchase.Items = purchaseItems;
            foreach (var item in cart)
                newPurchase.TotalPrice += item.Quantity * item.Product!.Price;
            await _unitOfWork.PurchaseCarts.Purchase(newPurchase);
            foreach (var item in cart)
            {
                item.Product!.Quantity -= item.Quantity;
                await _unitOfWork.Products.UpdateAsync(item.Product);
            }
            foreach(var item in cart)
            {
                _unitOfWork.PurchaseCarts.DeleteItem(item);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<PurchaseResponseDto>> GetPurchaseHistoryForUser(int userId)
        {
            var history = await _unitOfWork.PurchaseCarts.GetPurchasesFilteredAsync(userId: userId, includeItems: true);
            return _mapper.Map<List<PurchaseResponseDto>>(history);
        }












    }
} 