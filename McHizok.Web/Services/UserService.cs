using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Models;
using McHizok.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace McHizok.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
	{
        _userManager = userManager;
    }

    public Task<string> GenerateRegistrationToken(string to)
    {
        throw new NotImplementedException();
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
