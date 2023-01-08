using McHizok.Entities.Models;
using McHizok.Web.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace McHizok.Web.Data;

public class McHizokDbContext : IdentityDbContext<User>
{
    public McHizokDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new RoleConfiguration());
    }

    public DbSet<Registration>? Registrations { get; set; }
}
