namespace SuperMarketApi.Models
{
    public class Purchase
    {
        public int ID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public CategoryEnum Category { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }

        // Foreign key for User
        public int UserID { get; set; }
        public User? User { get; set; } // Navigation property
    }
} 