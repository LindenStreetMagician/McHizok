using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Models;
using McHizok.Services.Interfaces;
using McHizok.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace McHizok.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly McHizokDbContext _mcHizokDbContext;

    public UserService(UserManager<User> userManager, McHizokDbContext mcHizokDbContext)
	{
        _userManager = userManager;
        _mcHizokDbContext = mcHizokDbContext;
    }

    public async Task<string> GenerateRegistrationToken(string to)
    {
        string registrationToken;
        bool tokenExists;

        do
        {
            registrationToken = Guid.NewGuid().ToString("N");
            var registration = await _mcHizokDbContext.Registrations.FirstOrDefaultAsync(x => x.RegistrationToken == registrationToken);

            tokenExists = registration is not null;
        }
        while (tokenExists);

        var newRegistration = new Registration
        {
            RegistrationToken = registrationToken,
            To = to
        };

        await _mcHizokDbContext.AddAsync(newRegistration);
        await _mcHizokDbContext.SaveChangesAsync();

        return registrationToken;
    }


    public async Task<bool> ValidateRegistrationToken(string token)
    {
        var registration = await _mcHizokDbContext.Registrations.FirstOrDefaultAsync(x => x.RegistrationToken == token);

        return registration is not null;
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
