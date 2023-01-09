using McHizok.Entities.Models.Register;
using Microsoft.AspNetCore.Identity;

namespace McHizok.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterUser(RegisterRequest userForRegistration);
    Task<string> GenerateRegistrationToken(string to);
    Task<bool> ValidateRegistrationToken(string token);
}
