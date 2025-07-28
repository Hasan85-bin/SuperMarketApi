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

        public async Task<UserPurchaseDto> Send()
        {
            var sendingPurchase = await _unitOfWork.PurchaseCarts.GetLatestPendingRequest();
            if (sendingPurchase == null)
                throw new BadHttpRequestException("No New Purchase");
            
            // Business logic: Can only send purchases that are in Pending status
            if (sendingPurchase.Status != PurchaseStatus.Pending)
                throw new BadHttpRequestException($"Cannot send purchase with status: {sendingPurchase.Status}. Only Pending purchases can be sent.");
            
            sendingPurchase.Status = PurchaseStatus.Sent;
            _unitOfWork.PurchaseCarts.UpdatePurchase(sendingPurchase);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserPurchaseDto>(sendingPurchase);
        }

        public async Task<UserPurchaseDto> Deliver(int purchaseId)
        {
            var deliveringPurchase = await _unitOfWork.PurchaseCarts.GetPurchaseByIdAsync(purchaseId, true, true);
            if (deliveringPurchase == null)
                throw new BadHttpRequestException("Purchase Not Found");
            
            // Business logic: Can only deliver purchases that are in Sent status
            if (deliveringPurchase.Status != PurchaseStatus.Sent)
                throw new BadHttpRequestException($"Cannot deliver purchase with status: {deliveringPurchase.Status}. Only Sent purchases can be delivered.");
            
            deliveringPurchase.Status = PurchaseStatus.Delivered;
            _unitOfWork.PurchaseCarts.UpdatePurchase(deliveringPurchase);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserPurchaseDto>(deliveringPurchase);
        }

        public async Task<IEnumerable<UserPurchaseDto>> GetPurchasesFilteredAsync(
            PurchaseStatus? status = null,
            int? userId = null,
            string? userName = null,
            DateTime? from = null,
            DateTime? to = null
        )
        {
            var purchases = await _unitOfWork.PurchaseCarts.GetPurchasesFilteredAsync(status, userId, userName, from, to, includeUser: true, includeItems: true);
            return _mapper.Map<List<UserPurchaseDto>>(purchases);
        }

        public async Task<UserPurchaseDto?> GetPurchaseByIdAsync(int purchaseId)
        {
           
            var purchase = await _unitOfWork.PurchaseCarts.GetPurchaseByIdAsync(purchaseId, true, true);
            return purchase == null ? null : _mapper.Map<UserPurchaseDto>(purchase);
        }
    }
} 