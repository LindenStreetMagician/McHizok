using McHizok.Entities.Models.InputForm;
using McHizok.Web.Services.Interfaces;

namespace McHizok.Web.Services
{
    public class CouponInventoryService : ICouponInventoryService
    {
        public Task DeleteCoupon(string couponId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Coupon>> GetCoupons(string userId)
        {
            throw new NotImplementedException();
        }

        public Task SaveCoupon(string userId, Coupon coupon)
        {
            throw new NotImplementedException();
        }
    }
}
