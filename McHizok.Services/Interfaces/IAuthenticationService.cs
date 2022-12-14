using McHizok.Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace McHizok.Services.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
}
