using SuperMarketApi.Models;
using SuperMarketApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperMarketApi.DTOs.Staff;
using AutoMapper;

namespace SuperMarketApi.Services
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StaffService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Purchase> Send()
        {
            var sendingPurchase = await _unitOfWork.PurchaseCarts.GetLatestPendingRequest();
            if (sendingPurchase == null)
                throw new BadHttpRequestException("No New Purchase");
            sendingPurchase.Status = PurchaseStatus.Sent;
            _unitOfWork.PurchaseCarts.UpdatePurchase(sendingPurchase);
            await _unitOfWork.SaveChangesAsync();
            return sendingPurchase;
        }

        public async Task<Purchase> Deliver(int purchaseId)
        {
            var deliveringPurchase = await _unitOfWork.PurchaseCarts.GetPurchaseByIdAsync(purchaseId);
            if (deliveringPurchase == null)
                throw new BadHttpRequestException("Purchase Not Found");
            deliveringPurchase.Status = PurchaseStatus.Delivered;
            _unitOfWork.PurchaseCarts.UpdatePurchase(deliveringPurchase);
            await _unitOfWork.SaveChangesAsync();
            return deliveringPurchase;
        }

        public async Task<IEnumerable<UserPurchaseDto>> GetPurchasesFilteredAsync(
            PurchaseStatus? status = null,
            int? userId = null,
            string? userName = null,
            DateTime? from = null,
            DateTime? to = null
        )
        {
            var purchases = await _unitOfWork.PurchaseCarts.GetPurchasesFilteredAsync(status, userId, userName, from, to, includeUser: true);
            return _mapper.Map<List<UserPurchaseDto>>(purchases);
        }

        public async Task<UserPurchaseDto?> GetPurchaseByIdAsync(int purchaseId)
        {
           
            var purchase = await _unitOfWork.PurchaseCarts.GetPurchaseByIdAsync(purchaseId);
            return purchase == null ? null : _mapper.Map<UserPurchaseDto>(purchase);
        }
    }
} 