using McHizok.Entities.Models.Login;

namespace McHizok.Services.Interfaces;

public interface IAuthenticationService
{
    Task<bool> ValidateUser(LoginRequest userForAuthentication);
    Task<string> CreateToken();
}
