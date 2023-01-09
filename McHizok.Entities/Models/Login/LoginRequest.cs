using System.ComponentModel.DataAnnotations;

namespace McHizok.Entities.Models.Login;

public record LoginRequest
{
    [Required(ErrorMessage = "A felhasználó név megadása kötelező")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "A jelszó megadása kötelező")]
    public string? Password { get; init; }
}
