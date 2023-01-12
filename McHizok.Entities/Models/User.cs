using Microsoft.AspNetCore.Identity;

namespace McHizok.Entities.Models;

public class User : IdentityUser
{
    public string AccountFor { get; set; }
}
