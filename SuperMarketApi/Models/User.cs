

using SuperMarketApi.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public RoleEnum Role { get; set; } = RoleEnum.Customer;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }

    public List<Purchase> Purchases { get; set; } = new();
    public List<CartItem> CartItems { get; set; } = new();


}

public enum RoleEnum{
    Admin,
    Customer,
    Staff
}