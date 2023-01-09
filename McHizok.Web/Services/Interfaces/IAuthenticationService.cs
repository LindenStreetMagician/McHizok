using McHizok.Entities.Models.Login;
using Microsoft.AspNetCore.Identity;

namespace McHizok.Services.Interfaces;

public interface IAuthenticationService
{
    Task<bool> ValidateUser(LoginRequest userForAuthentication);
    Task<string> CreateToken();
}
