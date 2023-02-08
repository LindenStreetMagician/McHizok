using McHizok.Entities.DataTransferObjects;

namespace McHizok.Web.Services.Interfaces;

public interface ICouponInventoryService
{
    Task<IEnumerable<CouponDto>> GetCouponsForUserAsync(string userId);
    Task SaveCouponAsync(CouponDto coupon);
    Task DeleteCouponAsync(Guid couponId);
}
