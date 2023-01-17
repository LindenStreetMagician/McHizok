using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Exceptions;
using McHizok.Entities.Models.InputForm;
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

    public async Task<IEnumerable<CouponDto>> GetCoupons(string userId)
    {
        return await _mcHizokDbContext.CouponInventories
                                      .Where(x => x.User.Id == userId)
                                      .Select(c => new CouponDto(c.Id, c.CouponBase64, c.FileName, c.ExpiresAt, c.CouponCode))
                                      .ToListAsync();
    }

    public async Task SaveCoupon(string userId, CouponDto coupon)
    {
        var user = await _mcHizokDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            throw new UserNotFoundException(userId);

        var newCouponIventoryEntry = new CouponInventory
        {
            CouponBase64 = coupon.Base64Content,
            CouponCode = coupon.CouponCode,
            FileName = coupon.FileName,
            User = user
        };

        await _mcHizokDbContext.CouponInventories.AddAsync(newCouponIventoryEntry);
        await _mcHizokDbContext.SaveChangesAsync();
    }

    public async Task DeleteCoupon(Guid couponId)
    {
        var coupon = await _mcHizokDbContext.CouponInventories.FirstOrDefaultAsync(u => u.Id == couponId);

        if (coupon is null)
            throw new CouponNotFoundException(couponId);

        _mcHizokDbContext.CouponInventories.Remove(coupon);

        await _mcHizokDbContext.SaveChangesAsync();
    }
}
