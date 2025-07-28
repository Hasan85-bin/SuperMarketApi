using SuperMarketApi.DTOs.Staff;
using SuperMarketApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMarketApi.Services
{
    public interface IStaffService
    {     

        Task<IEnumerable<DTOs.Staff.UserPurchaseDto>> GetPurchasesFilteredAsync(
            PurchaseStatus? status = null,
            int? userId = null,
            string? userName = null,
            DateTime? from = null,
            DateTime? to = null
        );

        Task<DTOs.Staff.UserPurchaseDto?> GetPurchaseByIdAsync(int purchaseId);
        Task<UserPurchaseDto> Send();
        Task<UserPurchaseDto> Deliver(int purchaseId);
    }


} 