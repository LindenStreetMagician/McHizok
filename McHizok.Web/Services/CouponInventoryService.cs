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
                                      .Select(c => new CouponDto(c.Id, c.CouponBase64, c.ExpiresAt, c.CouponCode))
                                      .ToListAsync();
    }

    public Task SaveCoupon(string userId, Coupon coupon)
    {
        throw new NotImplementedException();
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
