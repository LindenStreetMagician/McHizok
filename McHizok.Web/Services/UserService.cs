using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Exceptions;
using McHizok.Entities.Models.Register;
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

    public async Task<string> GenerateRegistrationToken(string accountFor)
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
            AccountFor = accountFor
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

    public async Task<IdentityResult> RegisterUser(RegisterRequest registerRequest)
    {
        var registration = _mcHizokDbContext.Registrations.First(x => x.RegistrationToken == registerRequest.RegistrationToken);

        var user = new User
        {
            UserName = registerRequest.UserName,
            AccountFor = registration.AccountFor
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (result.Succeeded)
        {
            _mcHizokDbContext.Remove(registration);
            await _mcHizokDbContext.SaveChangesAsync();
        }

        return result;
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        return await _mcHizokDbContext.Users.Select(u => new UserDto(u.Id, u.UserName, u.AccountFor)).ToListAsync();
    }

    public async Task DeleteUser(string userId)
    {
        var user = await _mcHizokDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            throw new UserNotFoundException(userId);

        _mcHizokDbContext.Users.Remove(user);

        await _mcHizokDbContext.SaveChangesAsync();
    }
}
