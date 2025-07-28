using SuperMarketApi.DTOs.Staff;
using SuperMarketApi.Models;

namespace SuperMarketApi.DTOs.Cart
{
    public class PurchaseResponseDto
    {
        public int ID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public string PostCode { get; set; } = string.Empty;
        public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
        public List<PurchaseItemDto> Items { get; set; } = new();
    }


}
