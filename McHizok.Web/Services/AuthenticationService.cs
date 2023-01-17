using McHizok.Entities.Models.Login;
using McHizok.Services.Interfaces;
using McHizok.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace McHizok.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;
    private User? _user;

    public AuthenticationService(UserManager<User> userManager, IConfiguration configuration, ILogger<AuthenticationService> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> CreateTokenAsync()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaimsAsync();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public async Task<bool> ValidateUserAsync(LoginRequest loginRequest)
    {
        _user = await _userManager.FindByNameAsync(loginRequest.UserName);

        var isUserValid = (_user is not null && await _userManager.CheckPasswordAsync(_user, loginRequest.Password));

        if (!isUserValid)
        {
            _logger.LogWarning($"Invalid credentials were provided. Username: {loginRequest.UserName}");
        }

        return isUserValid;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaimsAsync()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.UserName),
            new Claim(ClaimTypes.NameIdentifier, _user.Id)
        };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );

        return tokenOptions;
    }
}
