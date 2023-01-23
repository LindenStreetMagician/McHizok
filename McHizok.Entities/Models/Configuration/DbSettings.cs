
namespace McHizok.Entities.Models.Configuration;

public class DbSettings
{
    public string Server { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Database { get; set; } = string.Empty;

    public string Uid { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
