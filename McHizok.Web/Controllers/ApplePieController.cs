using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Models.InputForm;
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

    public ApplePieController(IApplePieService applePieService, ICouponInventoryService couponInventoryService)
    {
        _applePieService = applePieService;
        _couponInventoryService = couponInventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCoupon([FromQuery] string blockCode)
    {
        var coupon = await _applePieService.GetApplePieCoupon(blockCode);

        return Ok(coupon);
    }

    [HttpGet("coupons")]
    public async Task<IActionResult> GetCouponsForUser([FromQuery] string Id)
    {
        var coupons = await _couponInventoryService.GetCoupons(Id);

        return Ok(coupons);
    }

    [HttpPost("coupons")]
    public async Task<IActionResult> SaveCouponToIventory([FromBody] CouponDto couponForIventory)
    {
        if (couponForIventory is null)
            BadRequest("CouponForIventory cannot be null.");

        await _couponInventoryService.SaveCoupon(couponForIventory);

        return Ok();
    }

    [HttpDelete("coupons")]
    public async Task<IActionResult> DeleteCoupon([FromQuery] Guid Id)
    {
        await _couponInventoryService.DeleteCoupon(Id);

        return NoContent();
    }
}
