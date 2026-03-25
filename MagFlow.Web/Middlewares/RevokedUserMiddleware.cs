using MagFlow.BLL.Helpers.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace MagFlow.Web.Middlewares
{
    public class RevokedUserMiddleware
    {
        private readonly RequestDelegate _next;

        public RevokedUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRevocationService userRevocationService)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId) && userRevocationService.IsRevoked(userId))
                {
                    await context.SignOutAsync(IdentityConstants.ApplicationScheme);
                    context.Response.Redirect("/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}
