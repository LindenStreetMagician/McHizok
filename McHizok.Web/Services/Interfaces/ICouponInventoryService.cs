using McHizok.Entities.DataTransferObjects;

namespace McHizok.Web.Services.Interfaces;

public interface ICouponInventoryService
{
    Task<IEnumerable<CouponDto>> GetCoupons(string userId);
    Task SaveCoupon(CouponDto coupon);
    Task DeleteCoupon(Guid couponId);
}
