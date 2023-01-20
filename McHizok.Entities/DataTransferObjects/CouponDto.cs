namespace McHizok.Entities.DataTransferObjects;

public record CouponDto {
    public Guid? Id { get; init; }
    public string? userId { get; init; }
    public string Base64Content { get; init; }
    public string CouponCode { get; init; }
    public string FileName { get; init; }
    public DateTime ExpiresAt { get; init; }
}