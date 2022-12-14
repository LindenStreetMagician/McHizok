using System.ComponentModel.DataAnnotations;

namespace McHizok.Entities.DataTransferObjects;

public record UserForRegistrationDto
{
    [Required(ErrorMessage = "A felhasználó név megadása kötelező")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "A jelszó megadása kötelező")]
    public string? Password { get; init; }

    public ICollection<string>? Roles { get; init; }
}
