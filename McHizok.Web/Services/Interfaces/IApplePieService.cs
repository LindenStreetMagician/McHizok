using McHizok.Entities.DataTransferObjects;

namespace McHizok.Services.Interfaces

{
    public interface IApplePieService
    {
        Task<CouponDto> GetApplePieCouponAsync(string blockCode);
    }
}
