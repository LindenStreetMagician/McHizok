using Microsoft.AspNetCore.Identity;

namespace McHizok.Web.Data;

public class User : IdentityUser
{
    public string AccountFor { get; set; }
}
