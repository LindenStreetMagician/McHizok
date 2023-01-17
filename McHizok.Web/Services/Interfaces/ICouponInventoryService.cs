using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Models.InputForm;

namespace McHizok.Web.Services.Interfaces;

public interface ICouponInventoryService
{
    Task<IEnumerable<CouponDto>> GetCoupons(string userId);
    Task SaveCoupon(string userId, Coupon coupon);
    Task DeleteCoupon(Guid couponId);
}
