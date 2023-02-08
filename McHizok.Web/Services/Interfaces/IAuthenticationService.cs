using McHizok.Entities.Models.Login;
using McHizok.Web.Data;

namespace McHizok.Web.Services.Interfaces;

public interface IAuthenticationService
{
    Task<(bool success, User? validatedUser)> ValidateUserAsync(LoginRequest userForAuthentication);
    Task<string> CreateTokenAsync(User user);
}
