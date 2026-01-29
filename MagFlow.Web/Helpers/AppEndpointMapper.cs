using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.Shared.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Claims;

namespace MagFlow.Web.Helpers
{
    public static class AppEndpointMapper
    {
        public static void MapEndpoints(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            app.MapAuth();
            
            app.MapGet("/{**path}", (HttpContext ctx) =>
            {
                var path = ctx.Request.Path.Value ?? "";
                if (path.StartsWith("/_framework") || path.StartsWith("/_content") || Path.HasExtension(path))
                    return Results.NotFound();

                return Results.Redirect("/NotFound");
            });
        }

        public static IEndpointConventionBuilder MapAuth(this IEndpointRouteBuilder endpoints)
        {
            var authGroup = endpoints.MapGroup("/Auth");

            authGroup.MapPost("/Login", async (
                [FromForm] SignInModel req,
                ILogger<Program> logger,
                IUserRepository repo,
                HttpContext http) =>
            {
                var ip = http.Connection.RemoteIpAddress?.MapToIPv4().ToString();
                logger.LogInformation($"User {req.Email} is attempting to login from IPv4 address: {ip}");
                
                var user = await repo.GetByEmailAsync(req.Email);
                if (user is null)
                {
                    logger.LogWarning($"Invalid login ({req.Email}) attempt from IPv4 address: {ip}");
                    return Results.Redirect("/Login?error=1");
                }

                var hasher = new PasswordHasher<ApplicationUser>();
                var verified = hasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    req.Password);

                if (verified == PasswordVerificationResult.Failed)
                {
                    logger.LogWarning($"Invalid login ({req.Email}) attempt from IPv4 address: {ip}");
                    return Results.Redirect("/Login?error=1");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(Claims.CompanyClaim, user.DefaultCompanyId?.ToString() ?? Guid.Empty.ToString())
                };

                var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProps = new AuthenticationProperties()
                {
                    IsPersistent = req.RememberMe
                };
                if (req.RememberMe)
                    authProps.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14);

                await http.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal,
                    authProps);

                logger.LogInformation($"User {req.Email} logged in via IPv4 address: {ip}");

                var returnUrl = "/lobby";
                return Results.Redirect(returnUrl);
            });

            authGroup.MapPost("/Logout", async (
               ClaimsPrincipal user,
               [FromServices] SignInManager<ApplicationUser> signInManager,
               [FromForm] string returnUrl) =>
            {
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect($"~/{returnUrl}");
            });

            return authGroup;
        }
    }
}
