﻿using McHizok.Entities.ErrorModel;
using McHizok.Entities.Models.Login;
using McHizok.Web.Services.Interfaces;
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

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest userForAuthentication)
    {
        if (userForAuthentication is null)
            return BadRequest("userForAuthentication cannot be null.");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var userValidationResult = await _authenticationService.ValidateUserAsync(userForAuthentication);

        if (!userValidationResult.success)
            return Unauthorized(new ErrorDetails { Message = "Invalid username or password.", StatusCode = 401 });

        return Ok(new { Token = await _authenticationService.CreateTokenAsync(userValidationResult.validatedUser!) });
    }
}
