using System.Security.Principal;

namespace SuperMarketApi.Models
{
    public class Product
    {
        public int ID { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public CategoryEnum Category { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; } // Stock/available quantity
    }

    public enum CategoryEnum
    {
        Dairy,
        HealthProducts,
        Drinks,
        Snacks,
        MeatOrFish,
        Fruits,
        Other
    }
}
