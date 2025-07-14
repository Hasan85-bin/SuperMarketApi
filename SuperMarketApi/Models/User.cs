

using SuperMarketApi.Models;

public class User
{
    public int ID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public RoleEnum Role { get; set; } = RoleEnum.Customer;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime? TokenExpiry { get; set; } = null;
    public Dictionary<Product, int> ShoppingCart { get; set; } = new();
}

public enum RoleEnum{
    Admin,
    Customer,
    Staff
}