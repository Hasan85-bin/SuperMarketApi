using System.Security.Principal;

namespace SuperMarketApi.Models
{
    public class Product
    {
        public int ID { get; set; }

        public string name { get; set; } = string.Empty;

        public string brand { get; set; } = string.Empty;

        public Category category { get; set; }

        public double price { get; set; }

    }

    public enum Category
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
