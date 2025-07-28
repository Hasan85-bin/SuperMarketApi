using Microsoft.AspNetCore.Mvc;
using SuperMarketApi.Services;
using AutoMapper;
using SuperMarketApi.DTOs.Cart;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SuperMarketApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SuperMarketApi.Controllers
{
    [ApiController]
    [Route("api/cart-purchase")]
    public class CartPurchaseController : ControllerBase
    {
        private readonly ICartPurchaseService _cartPurchaseService;
        private readonly IMapper _mapper;

        public CartPurchaseController(ICartPurchaseService cartPurchaseService, IMapper mapper)
        {
            _cartPurchaseService = cartPurchaseService;
            _mapper = mapper;
        }
        // Endpoints for cart and purchase logic will go here

        [HttpGet("cart")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CartItemResponseDto>>> GetCart()
        {
            var userId = GetUserIdFromClaims();
            var cart = await _cartPurchaseService.GetCartForUserAsync(userId);
            return Ok(cart);
        }

        [HttpGet("cart/{itemId}")]
        [Authorize]
        public async Task<ActionResult<CartItemResponseDto>> GetCartItem(int itemId)
        {
            var userId = GetUserIdFromClaims();
            var item = await _cartPurchaseService.GetItemAsync(userId, itemId);
            return Ok(item);
        }

        [HttpPost("cart")]
        [Authorize]
        public async Task<ActionResult> AddToCart([FromBody] CartItemCreateDto dto)
        {
            var userId = GetUserIdFromClaims();
            await _cartPurchaseService.AddToCartAsync(userId, dto);
            return Ok();
        }

        [HttpPut("cart/{itemId}")]
        [Authorize]
        public async Task<ActionResult> UpdateCartItem(int itemId, [FromBody] CartItemUpdateDto dto)
        {
            var userId = GetUserIdFromClaims();
            await _cartPurchaseService.UpdateItemAsync(userId, itemId, dto);
            return Ok();
        }

        [HttpDelete("cart/{itemId}")]
        [Authorize]
        public async Task<ActionResult> DeleteCartItem(int itemId)
        {
            var userId = GetUserIdFromClaims();
            await _cartPurchaseService.DeleteItemAsync(userId, itemId);
            return Ok();
        }

        [HttpPost("purchase")]
        [Authorize]
        public async Task<ActionResult> Purchase([FromBody] PurchaseRequestDto dto)
        {
            var userId = GetUserIdFromClaims();
            await _cartPurchaseService.Purchase(userId, dto.PostCode);
            return Ok();
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PurchaseResponseDto>>> GetPurchaseHistory()
        {
            var userId = GetUserIdFromClaims();
            var history = await _cartPurchaseService.GetPurchaseHistoryForUser(userId);
            return Ok(history);
        }

        

        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID in token.");
            }
            return userId;
        }

        //[HttpPost("add-to-cart")]
        //[Authorize]
        //public IActionResult AddToCart([FromQuery] int productId)
        //{
        //    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //        return Unauthorized();
        //    var userId = int.Parse(userIdClaim.ToString());
        //    _cartPurchaseService.AddToCart(productId, userId);
        //}
    }
} 