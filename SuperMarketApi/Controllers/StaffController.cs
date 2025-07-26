using Microsoft.AspNetCore.Mvc;
using SuperMarketApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using SuperMarketApi.DTOs.Staff;
using SuperMarketApi.Models;

namespace SuperMarketApi.Controllers
{
    [ApiController]
    [Route("api/staff")]
    [Authorize(Roles = "Staff,Admin")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }



        [HttpPost("send-latest")]
        public async Task<ActionResult<Purchase>> SendLatestPendingPurchase()
        {
            var purchase = await _staffService.Send();
            return Ok(purchase);
        }

        [HttpPost("deliver/{purchaseId}")]
        public async Task<ActionResult<Purchase>> DeliverPurchase(int purchaseId)
        {
            var purchase = await _staffService.Deliver(purchaseId);
            return Ok(purchase);
        }

        [HttpGet("purchases")]
        public async Task<ActionResult<IEnumerable<UserPurchaseDto>>> GetPurchasesFiltered(
            [FromQuery] PurchaseStatus? status,
            [FromQuery] int? userId,
            [FromQuery] string? userName,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to
        )
        {
            var purchases = await _staffService.GetPurchasesFilteredAsync(status, userId, userName, from, to);
            return Ok(purchases);
        }

        [HttpGet("purchases/{purchaseId}")]
        public async Task<ActionResult<UserPurchaseDto>> GetPurchaseById(int purchaseId)
        {
            var purchase = await _staffService.GetPurchaseByIdAsync(purchaseId);
            if (purchase == null)
                return NotFound();
            return Ok(purchase);
        }
    }
} 