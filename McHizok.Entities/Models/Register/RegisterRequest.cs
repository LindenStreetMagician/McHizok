using System.ComponentModel.DataAnnotations;

namespace McHizok.Entities.Models.Register;

public record RegisterRequest
{
    [Required(ErrorMessage = "The username is a required field.")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "The password is a required field.")]
    public string? Password { get; init; }

    public string? RegistrationToken { get; set; }
}
