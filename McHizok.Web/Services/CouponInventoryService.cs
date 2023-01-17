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

    public Task<IEnumerable<Coupon>> GetCoupons(string userId)
    {
        throw new NotImplementedException();
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
