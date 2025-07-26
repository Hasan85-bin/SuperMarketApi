using System;
using System.Collections.Generic;

namespace SuperMarketApi.DTOs.Staff
{
    public class UserPurchaseDto
    {
        public int ID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public string PostCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public StaffUserDto User { get; set; } = new();
        public List<PurchaseItemDto> Items { get; set; } = new();
    }

    public class StaffUserDto
    {
        public int ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }

    public class PurchaseItemDto
    {
        public int ID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
} 