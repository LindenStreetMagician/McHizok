namespace McHizok.Entities.Exceptions;

public class CouponNotFoundException : NotFoundException
{
    public CouponNotFoundException(Guid couponId) 
        : base($"The coupon with the Id: {couponId} doesn't exist")
    {
    }
}
