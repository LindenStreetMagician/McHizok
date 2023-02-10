using McHizok.Web.Data;
using Microsoft.AspNetCore.Identity;
using AuthenticationService = McHizok.Web.Services.AuthenticationService;
using Microsoft.Extensions.Logging;
using Moq;
using McHizok.Entities.Models.Configuration;
using Microsoft.Extensions.Options;
using McHizok.Entities.Models.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace McHizok.Tests;

public class AuthenticationServiceTests
{
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<ILogger<AuthenticationService>> _loggerMock;
    private AuthenticationService _authenticationService;
    private Mock<IOptions<JwtSettingsOptions>> _jwtSettingsOptionsMock;

    public AuthenticationServiceTests()
	{
        _loggerMock = new Mock<ILogger<AuthenticationService>>();

        var mockUserStore = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
        _jwtSettingsOptionsMock = new Mock<IOptions<JwtSettingsOptions>>();

        var _jwtSettingsOptionsValue = new JwtSettingsOptions
        {
            ValidAudience = "TestAudience",
            ValidIssuer = "TestIssuer",
            Expires = 60
        };

        _jwtSettingsOptionsMock.Setup(jwtOptions => jwtOptions.Value).Returns(_jwtSettingsOptionsValue);

        _authenticationService = new AuthenticationService(_userManagerMock.Object, _loggerMock.Object, _jwtSettingsOptionsMock.Object);
    }

    [Fact]
    public async Task ValidateUserAsync_UserValid_ReturnsTrue()
    {
        var loginRequest = new LoginRequest
        {
            UserName = "TestUser",
            Password = "TestPassword"
        };

        var userFromDb = new User { UserName = "TestUser" };

        _userManagerMock.Setup(um => um.FindByNameAsync(loginRequest.UserName)).ReturnsAsync(userFromDb);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(userFromDb, loginRequest.Password)).ReturnsAsync(true);

        var validationResult = await _authenticationService.ValidateUserAsync(loginRequest);

        Assert.True(validationResult.success);
    }

    [Fact]
    public async Task ValidateUserAsync_InvalidPassword_ReturnsFalse()
    {
        var loginRequest = new LoginRequest
        {
            UserName = "TestUser",
            Password = "InvalidPassword"
        };

        var userFromDb = new User { UserName = "TestUser" };

        _userManagerMock.Setup(um => um.FindByNameAsync(loginRequest.UserName)).ReturnsAsync(userFromDb);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(userFromDb, loginRequest.Password)).ReturnsAsync(false);

        var validationResult = await _authenticationService.ValidateUserAsync(loginRequest);

        _loggerMock.VerifyLog(logger => logger.LogWarning($"Invalid credentials were provided. Username: {loginRequest.UserName}"));

        Assert.False(validationResult.success);
    }

    [Fact]
    public async Task CreateTokenAsync_RegularUserCalls_ReturnsJwtWithoutAdminRole()
    {
        var regularUserFromDb = new User { UserName = "TestUser" };
        _userManagerMock.Setup(um => um.GetRolesAsync(regularUserFromDb)).ReturnsAsync(new List<string>());

        var token = await _authenticationService.CreateTokenAsync(regularUserFromDb);

        var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Null(decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value == "Admin"));
    }

    [Fact]
    public async Task CreateTokenAsync_AdminUserCalls_ReturnsJwtWithAdminRole()
    {
        var regularUserFromDb = new User { UserName = "TestUser" };
        _userManagerMock.Setup(um => um.GetRolesAsync(regularUserFromDb)).ReturnsAsync(new List<string>() { "Admin" });

        var token = await _authenticationService.CreateTokenAsync(regularUserFromDb);

        var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.NotNull(decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value == "Admin"));
    }
}
