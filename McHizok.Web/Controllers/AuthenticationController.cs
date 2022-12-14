using McHizok.Entities.DataTransferObjects;
using McHizok.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace McHizok.Web.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    {
        if (userForRegistration is null)
            return BadRequest("userForRegistrationDto cannot be null.");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var result = await _authenticationService.RegisterUser(userForRegistration);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }
}
