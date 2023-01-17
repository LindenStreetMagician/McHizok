namespace McHizok.Entities.DataTransferObjects;

public record CouponDto(Guid Id, string Base64Content, DateTime ExpiresAt, string CouponCode);