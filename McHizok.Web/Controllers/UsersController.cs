using McHizok.Entities.Exceptions;
using McHizok.Entities.Models.Register;
using McHizok.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace McHizok.Web.Controllers;

[Route("/api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("generate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRegistrationLink([FromQuery(Name = "account_for")] string accountFor)
    {
        var registrationToken = await _userService.GenerateRegistrationTokenAsync(accountFor);

        return Ok(registrationToken);
    }

    [HttpGet("validate")]
    public async Task<IActionResult> ValidateRegistrationLink([FromQuery] string token)
    {
        var isTokenValid = await _userService.ValidateRegistrationTokenAsync(token);

        return Ok(isTokenValid);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsersAsync();

        return Ok(users);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest userForRegistration)
    {
        if (userForRegistration is null)
            return BadRequest("userForRegistrationDto cannot be null.");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var result = await _userService.RegisterUserAsync(userForRegistration);

        if (!result.Succeeded)
        {
            _logger.LogError("User creation failed", result.Errors);
            throw new InvalidUsernameBadRequestException();
        }

        return StatusCode(201);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser([FromQuery] string userId)
    {
        await _userService.DeleteUserAsync(userId);

        return NoContent();
    }


}
