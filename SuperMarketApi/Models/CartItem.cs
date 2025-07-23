using System;

namespace SuperMarketApi.Models
{
    public class CartItem
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; } 


        // Navigation properties
        public User? User { get; set; }
        public Product? Product { get; set; }
    }
} 