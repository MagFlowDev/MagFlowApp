using MagFlow.Domain.Core;
using MagFlow.Shared.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MagFlow.Web.Helpers
{
    public static class AppEndpointMapper
    {
        public static void MapEndpoints(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            app.MapAuth();
        }

        public static IEndpointConventionBuilder MapAuth(this IEndpointRouteBuilder endpoints)
        {
            var authGroup = endpoints.MapGroup("/Auth");

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
