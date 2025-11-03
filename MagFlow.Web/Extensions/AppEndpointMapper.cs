using MagFlow.Domain.Core;
using MagFlow.Shared.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace MagFlow.Web.Extensions
{
    public static class AppEndpointMapper
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapLogin();

            app.MapGet("/test-set-cookie", (HttpContext http) =>
            {
                http.Response.Cookies.Append("mf_test_cookie", "1", new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false, // tymczasowo false, żeby działało na http://localhost
                    SameSite = SameSiteMode.Lax,
                    Path = "/"
                });
                return Results.Text("cookie set");
            });
        }

        private static void MapLogin(this WebApplication app)
        {
            app.MapPost("/login", async (HttpContext http,
                SignInManager<ApplicationUser> signInManager,
                UserManager<ApplicationUser> userManager,
                SignInModel login) =>
            {
                var user = await userManager.FindByEmailAsync(login.Email);
                if (user is null)
                    return Results.Unauthorized();

                if (!await userManager.CheckPasswordAsync(user, login.Password))
                    return Results.Unauthorized();

                var principal = await signInManager.CreateUserPrincipalAsync(user);

                await http.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12)
                    });

                return Results.Ok();
            });
        }
    }
}
