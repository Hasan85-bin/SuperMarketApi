using System.ComponentModel.DataAnnotations;

namespace SuperMarketApi.Models
{
    public class Purchase
    {
        public int ID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public string PostCode { get; set; } = string.Empty;
        public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
        public int UserID { get; set; }
        public User? User { get; set; }
        public List<PurchaseItem> Items { get; set; } = new();
    }

    public class PurchaseItem
    {
        public int ID { get; set; }
        public int PurchaseID { get; set; }
        public Purchase? Purchase { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public CategoryEnum Category { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public enum PurchaseStatus
    {
        Pending, // Not yet sent
        Sent,    // Sent to user
        Delivered // Delivered to user
    }
} 