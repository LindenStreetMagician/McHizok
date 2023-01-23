using McHizok.Entities.DataTransferObjects;

namespace McHizok.Web.Services.Interfaces;
public interface IApplePieService
{
    Task<CouponDto> GetApplePieCouponAsync(string blockCode);
}
