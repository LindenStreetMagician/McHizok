
using McHizok.Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace McHizok.Services.Interfaces;

public interface IAccountService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
}
