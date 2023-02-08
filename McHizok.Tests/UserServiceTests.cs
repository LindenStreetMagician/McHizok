using McHizok.Entities.Exceptions;
using McHizok.Entities.Models.Register;
using McHizok.Web.Data;
using McHizok.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace McHizok.Tests
{
    public class UserServiceTests
    {
        private McHizokDbContext _mcHizokDbContext;
        private Mock<UserManager<User>> _userManagerMock;
        private UserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<McHizokDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mcHizokDbContext = new McHizokDbContext(options);
            _mcHizokDbContext.Database.EnsureCreated();

            var mockUserStore = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            _userService = new UserService(_userManagerMock.Object, _mcHizokDbContext);
        }

        [Fact]
        public async Task GenerateRegistrationTokenAsync_HappyPath_ReturnsRegistrationTokenGuid()
        {
            var registrationToken = await _userService.GenerateRegistrationTokenAsync("testUser");

            var registrationFromDb = _mcHizokDbContext.Registrations.FirstOrDefault(r => r.RegistrationToken == registrationToken);

            Assert.NotNull(registrationFromDb);
        }

        [Fact]
        public async Task ValidateRegistrationTokenAsync_TokenExistsInDatabase_ReturnsTrue()
        {
            var registrationToken = Guid.NewGuid().ToString("N");

            var newRegistration = new Registration
            {
                RegistrationToken = registrationToken,
                AccountFor = "testUser"
            };

            await _mcHizokDbContext.Registrations.AddAsync(newRegistration);
            await _mcHizokDbContext.SaveChangesAsync();

            var isRegistrationTokenValid = await _userService.ValidateRegistrationTokenAsync(registrationToken);

            Assert.True(isRegistrationTokenValid);
        }

        [Fact]
        public async Task ValidateRegistrationTokenAsync_TokenDoesNotExistInDatabase_ReturnsFalse()
        {
            var registrationToken = Guid.NewGuid().ToString("N");

            var isRegistrationTokenValid = await _userService.ValidateRegistrationTokenAsync(registrationToken);

            Assert.False(isRegistrationTokenValid);
        }

        [Fact]
        public async Task RegisterUserAsync_TokenDoesNotExistInDatabase_ThrowsRegistrationTokenNotProvidedBadRequestException()
        {
            var userToRegister = new RegisterRequest
            {
                UserName = "Test",
                Password = "Test"
            };

            var thrownException = await Assert.ThrowsAsync<RegistrationTokenNotProvidedBadRequestException>(() => _userService.RegisterUserAsync(userToRegister));
            Assert.Equal("The provided token was invalid", thrownException.Message);
        }

        [Fact]
        public async Task RegisterUserAsync_TokenExistUserInfoAreOkay_ReturnsIdentityResultSucceess()
        {
            var registrationToken = await CreateRegistrationToken();

            var userToRegister = new RegisterRequest
            {
                UserName = "Test",
                Password = "Test",
                RegistrationToken = registrationToken
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var result = await _userService.RegisterUserAsync(userToRegister);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task RegisterUserAsync_TokenExistUserNameContainsIllegalChar_ReturnsIdentityResultSucceess()
        {
            var registrationToken = await CreateRegistrationToken();

            var userToRegister = new RegisterRequest
            {
                UserName = "Testßser",
                Password = "Test",
                RegistrationToken = registrationToken
            };

            var identityError = new IdentityError { Code = "InvalidUserName", Description = "Username is invalid, can only contain letters or digits." };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            var result = await _userService.RegisterUserAsync(userToRegister);

            Assert.False(result.Succeeded);
            Assert.Contains<IdentityError>(identityError, result.Errors);
        }

        [Fact]
        public async Task GetUsersAsync_HappyPath_ReturnsUsersWithoutAdminRole()
        {
            var adminUser = new User { UserName = "Administrator", AccountFor = "Myself" };
            var regularUser = new User { UserName = "RegularUser", AccountFor = "RegularUser" };

            await _mcHizokDbContext.Users.AddAsync(adminUser);
            await _mcHizokDbContext.Users.AddAsync(regularUser);
            await _mcHizokDbContext.SaveChangesAsync();

            _userManagerMock.Setup(um => um.GetUsersInRoleAsync("Admin")).ReturnsAsync(new List<User> { adminUser });

            var result = await _userService.GetUsersAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task DeleteUserAsync_UserDoesNotExist_ThrowsUserNotFoundException()
        {
            var thrownException = await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteUserAsync("notExistingUserId"));

            Assert.Equal("The user with id: notExistingUserId doesn't exist in the database", thrownException.Message);
        }

        [Fact]
        public async Task DeleteUserAsync_UserExists_UserIsDeleted()
        {
            var regularUser = new User { UserName = "RegularUser", AccountFor = "RegularUser" };
            await _mcHizokDbContext.Users.AddAsync(regularUser);
            await _mcHizokDbContext.SaveChangesAsync();

            await _userService.DeleteUserAsync(regularUser.Id);

            Assert.DoesNotContain(regularUser, _mcHizokDbContext.Users);
        }

        private async Task<string> CreateRegistrationToken()
        {
            var registrationToken = Guid.NewGuid().ToString("N");

            var newRegistration = new Registration
            {
                RegistrationToken = registrationToken,
                AccountFor = "testUser",
            };

            await _mcHizokDbContext.Registrations.AddAsync(newRegistration);
            await _mcHizokDbContext.SaveChangesAsync();

            return registrationToken;
        }
    }
}