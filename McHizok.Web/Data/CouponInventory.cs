using System.ComponentModel.DataAnnotations.Schema;

namespace McHizok.Web.Data;

[Table("CouponInventory")]
public class CouponInventory
{
    public Guid Id { get; set; }
    public string CouponBase64 { get; set; }
    public string CouponCode { get; set; }
    public DateTime ExpiresAt { get; set; } = DateTime.Now.AddDays(7);

    public User User { get; set; }
}
