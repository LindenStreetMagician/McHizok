
using McHizok.Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace McHizok.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
    Task<string> GenerateRegistrationToken(string to);
    Task<bool> ValidateRegistrationToken(string token);
}
