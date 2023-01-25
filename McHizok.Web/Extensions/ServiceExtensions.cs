using McHizok.Entities.Models.Configuration;
using McHizok.Web.Services;
using McHizok.Web.Services.Interfaces;
using McHizok.Web.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AspNetCoreRateLimit;
using OpenQA.Selenium.DevTools.V105.Profiler;
using McHizok.Entities.ErrorModel;

namespace McHizok.Web.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IApplePieService, ApplePieService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICouponInventoryService, CouponInventoryService>();
    }

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<McHizokDbContext>(options =>
        {
            var dbSettings = configuration.GetSection("DbSettings").Get<DbSettings>();

            var connectionString = $"server={dbSettings.Server};port={dbSettings.Port};database={dbSettings.Database};uid={dbSettings.Uid};password={dbSettings.Password}";

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

    public static void ConfigureRateLimitingOptions(this IServiceCollection services)
    {
        services.AddMemoryCache();

        var rateLimitRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "post:/api/users/register",
                Limit = 3,
                Period = "15m"
            },
            new RateLimitRule
            {
                Endpoint = "get:/api/users/validate",
                Limit = 3,
                Period = "15m"
            }
        };

        services.Configure<IpRateLimitOptions>(opt => {
            opt.EnableEndpointRateLimiting= true;
            opt.QuotaExceededMessage = "Woah-Woah-Woah! Slow the 🦆 down with those requests!";
            opt.GeneralRules = rateLimitRules;
        });
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    }

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Environment.GetEnvironmentVariable("SECRET");

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
    }
}
