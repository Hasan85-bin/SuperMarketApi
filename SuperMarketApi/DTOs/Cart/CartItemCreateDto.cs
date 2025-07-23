using System.ComponentModel.DataAnnotations;

namespace SuperMarketApi.DTOs.Cart
{
    public class CartItemCreateDto
    {
        public int ProductID { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
} 