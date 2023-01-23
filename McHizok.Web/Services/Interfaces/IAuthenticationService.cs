using McHizok.Entities.Models.Login;

namespace McHizok.Web.Services.Interfaces;

public interface IAuthenticationService
{
    Task<bool> ValidateUserAsync(LoginRequest userForAuthentication);
    Task<string> CreateTokenAsync();
}
