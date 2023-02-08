namespace McHizok.Entities.Models.Configuration;

public class JwtSettingsOptions
{
    public const string JwtSettings = "JwtSettings";

    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public int Expires { get; set; }
}
