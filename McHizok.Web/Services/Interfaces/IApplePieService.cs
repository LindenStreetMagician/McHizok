using McHizok.Entities.Models.InputForm;

namespace McHizok.Services.Interfaces

{
    public interface IApplePieService
    {
        Task<Coupon> GetApplePieCoupon(string blockCode);
    }
}
