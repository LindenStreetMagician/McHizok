using McHizok.Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace McHizok.Services.Interfaces;

public interface IAuthenticationService
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuthentication);
    Task<string> CreateToken();
}
