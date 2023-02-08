using McHizok.Entities.Models.Login;
using McHizok.Web.Services.Interfaces;
using McHizok.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using McHizok.Entities.Models.Configuration;
using Microsoft.Extensions.Options;

namespace McHizok.Web.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtSettingsOptions _jwtSettings;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(UserManager<User> userManager, ILogger<AuthenticationService> logger, IOptions<JwtSettingsOptions> jwtSettings)
    {
        _userManager = userManager;
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string> CreateTokenAsync(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaimsAsync(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public async Task<(bool success, User? validatedUser)> ValidateUserAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.UserName);

        var isUserValid = (user is not null && await _userManager.CheckPasswordAsync(user, loginRequest.Password));

        if (!isUserValid)
        {
            _logger.LogWarning($"Invalid credentials were provided. Username: {loginRequest.UserName}");
        }

        return (isUserValid, user);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
            (
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.Expires)),
                signingCredentials: signingCredentials
            );

        return tokenOptions;
    }
}
