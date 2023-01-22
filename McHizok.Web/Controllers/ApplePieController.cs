﻿using McHizok.Entities.DataTransferObjects;
using McHizok.Services.Interfaces;
using McHizok.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace McHizok.Web.Controllers;

[Route("/api/applepies")]
[ApiController]
[Authorize]
public class ApplePieController : ControllerBase
{
    private readonly IApplePieService _applePieService;
    private readonly ICouponInventoryService _couponInventoryService;
    private readonly ILogger<ApplePieController> _logger;

    public ApplePieController(IApplePieService applePieService, ICouponInventoryService couponInventoryService, ILogger<ApplePieController> logger)
    {
        _applePieService = applePieService;
        _couponInventoryService = couponInventoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetCoupon([FromQuery] string blockCode)
    {
        var coupon = await _applePieService.GetApplePieCouponAsync(blockCode);

        _logger.LogInformation($"Coupon was created for {User.Identity.Name} with code: {blockCode}");

        return Ok(coupon);
    }

    [HttpGet("coupons")]
    public async Task<IActionResult> GetCouponsForUser([FromQuery] string userId)
    {
        var coupons = await _couponInventoryService.GetCouponsAsync(userId);

        return Ok(coupons);
    }

    [HttpPost("coupons")]
    public async Task<IActionResult> SaveCouponToIventory([FromBody] CouponDto couponForIventory)
    {
        if (couponForIventory is null)
            BadRequest("CouponForIventory cannot be null.");

        await _couponInventoryService.SaveCouponAsync(couponForIventory);

        return Ok();
    }

    [HttpDelete("coupons")]
    public async Task<IActionResult> DeleteCoupon([FromQuery] Guid couponId)
    {
        await _couponInventoryService.DeleteCouponAsync(couponId);

        return NoContent();
    }
}
