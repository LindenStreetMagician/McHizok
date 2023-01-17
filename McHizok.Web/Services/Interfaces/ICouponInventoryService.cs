using McHizok.Entities.Models.InputForm;

namespace McHizok.Web.Services.Interfaces;

public interface ICouponInventoryService
{
    Task<IEnumerable<Coupon>> GetCoupons(string userId);
    Task SaveCoupon(string userId, Coupon coupon);
    Task DeleteCoupon(Guid couponId);
}
