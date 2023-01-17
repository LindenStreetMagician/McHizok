using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Models.Register;
using Microsoft.AspNetCore.Identity;

namespace McHizok.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(RegisterRequest userForRegistration);
    Task<string> GenerateRegistrationTokenAsync(string accountFor);
    Task<bool> ValidateRegistrationTokenAsync(string token);
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task DeleteUserAsync(string userId);
}
