﻿using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Exceptions;
using McHizok.Web.Data;
using McHizok.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace McHizok.Web.Services;

public class CouponInventoryService : ICouponInventoryService
{
    private readonly McHizokDbContext _mcHizokDbContext;

    public CouponInventoryService(McHizokDbContext mcHizokDbContext)
    {
        _mcHizokDbContext = mcHizokDbContext;
    }

    public async Task<IEnumerable<CouponDto>> GetCouponsAsync(string userId)
    {
        return await _mcHizokDbContext.CouponInventories
                                      .Where(x => x.User.Id == userId)
                                      .Select(c => new CouponDto(c.Id, c.CouponBase64, c.FileName, c.ExpiresAt, c.CouponCode, c.User.Id))
                                      .ToListAsync();
    }

    public async Task SaveCouponAsync(CouponDto couponForInventory)
    {
        var user = await _mcHizokDbContext.Users.FirstOrDefaultAsync(u => u.Id == couponForInventory.UserId);

        if (user is null)
            throw new UserNotFoundException(couponForInventory.UserId);

        var newCouponIventoryEntry = new CouponInventory
        {
            CouponBase64 = couponForInventory.Base64Content,
            CouponCode = couponForInventory.CouponCode,
            FileName = couponForInventory.FileName,
            User = user
        };

        await _mcHizokDbContext.CouponInventories.AddAsync(newCouponIventoryEntry);
        await _mcHizokDbContext.SaveChangesAsync();
    }

    public async Task DeleteCouponAsync(Guid couponId)
    {
        var coupon = await _mcHizokDbContext.CouponInventories.FirstOrDefaultAsync(u => u.Id == couponId);

        if (coupon is null)
            throw new CouponNotFoundException(couponId);

        _mcHizokDbContext.CouponInventories.Remove(coupon);

        await _mcHizokDbContext.SaveChangesAsync();
    }
}