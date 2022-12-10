﻿using McHizok.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace McHizok.Web.Controllers;

[ApiController]
[Route("/api/applepies")]
public class ApplePieController : ControllerBase
{
    private readonly IApplePieService _applePieService;
    public ApplePieController(IApplePieService applePieService)
    {
        _applePieService = applePieService;
    }

    //TODO: Add block code format validation
    [HttpGet]
    public async Task<IActionResult> GetCoupon([FromQuery]string blockCode)
    {
        var coupon = await _applePieService.GetApplePieCoupon(blockCode);

        return File(coupon.Content, "image/jpeg", coupon.FileName);
    }
}
