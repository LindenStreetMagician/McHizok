using McHizok.Entities.Models;
using McHizok.Services;
using McHizok.Services.Interfaces;
using McHizok.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace McHizok.Web.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IApplePieService, ApplePieService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

    public static void ConfigureSqlContext(this IServiceCollection services) =>
        services.AddDbContext<McHizokDbContext>(options =>
        {
            //TODO:Move to IOptions
            var connectionString = "server=localhost;port=3306;database=mchizok;uid=root";
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 10;
        })
        .AddEntityFrameworkStores<McHizokDbContext>()
        .AddDefaultTokenProviders();
    }
}
