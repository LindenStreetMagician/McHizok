using McHizok.Entities.Models;

namespace McHizok.Services.Interfaces
{
    public interface IApplePieService
    {
        Task<Coupon> GetApplePieCoupon(string blockCode);
    }
}
