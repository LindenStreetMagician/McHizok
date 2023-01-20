namespace McHizok.Entities.DataTransferObjects;

public record CouponDto(Guid? couponId, string Base64Content, string FileName, DateTime ExpiresAt, string CouponCode, string? UserId);