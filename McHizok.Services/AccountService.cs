using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Models;
using McHizok.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace McHizok.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;

    public AccountService(UserManager<User> userManager)
	{
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var user = new User
        {
            UserName = userForRegistration.UserName
        };

        var result = await _userManager.CreateAsync(user, userForRegistration.Password);

        return result;
    }
}
