using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace McHizok.Web.Controllers;

[Route("/api/accounts")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AccountsController : ControllerBase
{

    public AccountsController()
    {
    }

    [HttpGet("generate")]
    public async Task<IActionResult> CreateRegistrationLink()
    {
        return Ok("CreateRegLink");
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok("GetUsers");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        return NoContent();
    }


}
