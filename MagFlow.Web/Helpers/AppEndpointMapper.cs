using MagFlow.BLL.Helpers.Auth;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CoreScope;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Domain.CoreScope;
using MagFlow.Shared.Attributes;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Auth;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Ocsp;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;

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


            app.MapGet("/ForceLogout", async (HttpContext context,
                [FromServices] SignInManager<ApplicationUser> signInManager,
                [FromServices] AuthenticationStateProvider authProvider,
                IMemoryCache cache) =>
            {
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value?.ToString();
                if (!string.IsNullOrEmpty(userId))
                {
                    var cacheKey = $"user_permissions_{userId}";
                    if(cache.TryGetValue(cacheKey, out var value))
                        cache.Remove(cacheKey);
                }
                await context.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Results.Redirect("/Login");
            });
        }

        public static IEndpointConventionBuilder MapAuth(this IEndpointRouteBuilder endpoints)
        {
            var authGroup = endpoints.MapGroup("/Auth");

            authGroup.MapGet("/SwitchCompany", async (HttpContext context,
               [FromServices] SignInManager<ApplicationUser> signInManager,
               [FromServices] UserManager<ApplicationUser> userManager,
               ILogger<Program> logger,
               IUserRepository repo,
               IEventService eventService,
               IOptions<IdentityOptions> identityOptions) =>
            {
                var ip = context.Connection.RemoteIpAddress?.MapToIPv4().ToString();
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
                var user = await userManager.FindByIdAsync(userId);
                if (user is null || user.RemovedAt.HasValue)
                {
                    return Results.Redirect("/Login?error=1");
                }

                var principal = await signInManager.CreateUserPrincipalAsync(user);
                var identity = (ClaimsIdentity)principal.Identity;
                if (!identity.HasClaim(c => c.Type == identityOptions.Value.ClaimsIdentity.SecurityStampClaimType))
                    identity.AddClaim(new Claim(identityOptions.Value.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp));
                if (!identity.HasClaim(c => c.Type == Claims.CompanyClaim))
                    identity.AddClaim(new Claim(Claims.CompanyClaim, user.DefaultCompanyId?.ToString() ?? Guid.Empty.ToString()));

                await signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, principal, new AuthenticationProperties { IsPersistent = true });

                try
                {
                    user.LastLogin = DateTime.UtcNow;
                    repo.UpdateAsync(user);
                    eventService.AddEventAsync(user.Id, Enums.EventLogCategory.Logging, Enums.EventLogLevel.INFO,
                        nameof(Langs.UserLoggedIn), string.Empty, ip, string.Empty);
                }
                catch { }

                var returnUrl = "/lobby";
                return Results.Redirect(returnUrl);
            });

            authGroup.MapPost("/Login", async (
                [FromForm] SignInModel req,
                [FromServices] SignInManager<ApplicationUser> signInManager,
                [FromServices] UserManager<ApplicationUser> userManager,
                ILogger<Program> logger,
                IUserRepository repo,
                IEventService eventService,
                IUserRevocationService revocationService,
                HttpContext http,
                IOptions<IdentityOptions> identityOptions) =>
            {
                var ip = http.Connection.RemoteIpAddress?.MapToIPv4().ToString();
                logger.LogInformation($"User {req.Email} is attempting to login from IPv4 address: {ip}");

                var user = await userManager.FindByEmailAsync(req.Email);
                if (user is null || user.RemovedAt.HasValue || !user.IsActive)
                {
                    logger.LogWarning($"Invalid login ({req.Email}) attempt from IPv4 address: {ip}");
                    return Results.Redirect("/Login?error=1");
                }

                var result = await signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    logger.LogWarning($"Invalid login ({req.Email}) attempt from IPv4 address: {ip}");
                    return Results.Redirect("/Login?error=1");
                }

                var principal = await signInManager.CreateUserPrincipalAsync(user);
                var identity = (ClaimsIdentity)principal.Identity;
                if (!identity.HasClaim(c => c.Type == identityOptions.Value.ClaimsIdentity.SecurityStampClaimType))
                    identity.AddClaim(new Claim(identityOptions.Value.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp));
                if (!identity.HasClaim(c => c.Type == Claims.CompanyClaim))
                    identity.AddClaim(new Claim(Claims.CompanyClaim, user.DefaultCompanyId?.ToString() ?? Guid.Empty.ToString()));

                var authProps = new AuthenticationProperties()
                {
                    IsPersistent = req.RememberMe
                };
                if (req.RememberMe)
                    authProps.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14);

                await signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, principal, authProps);
                revocationService.UnrevokeUser(user.Id.ToString());

                logger.LogInformation($"User {req.Email} logged in via IPv4 address: {ip}");

                try
                {
                    user.LastLogin = DateTime.UtcNow;
                    repo.UpdateAsync(user);
                    eventService.AddEventAsync(user.Id, Enums.EventLogCategory.Logging, Enums.EventLogLevel.INFO,
                        nameof(Langs.UserLoggedIn), string.Empty, ip, string.Empty);
                }
                catch { }

                var returnUrl = "/lobby";
                return Results.Redirect(returnUrl);
            });

            authGroup.MapPost("/Register", async (
                [FromBody] SignUpModel req,
                [FromServices] SignInManager<ApplicationUser> signInManager,
                ILogger<Program> logger,
                IUserRepository repo,
                HttpContext http) =>
            {
                var ip = http.Connection.RemoteIpAddress?.MapToIPv4().ToString();
                logger.LogInformation($"Attemption to create user from IPv4 address: {ip}");

                var userIdString = http?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdString, out var userId))
                {
                    logger.LogError($"Cannot get actual user");
                    return Enums.Result.Error;
                }
                var user = await repo.GetByIdAsync(userId);
                if(user == null)
                {
                    logger.LogError($"Cannot get actual user");
                    return Enums.Result.Error;
                }

                var tempUser = await repo.GetByEmailAsync(req.Email);
                if (tempUser != null)
                {
                    logger.LogError($"Cannot create user because already exists({req.Email})");
                    return Enums.Result.Error;
                }
                
                if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.FirstName) || string.IsNullOrEmpty(req.LastName) || string.IsNullOrEmpty(req.Password))
                {
                    logger.LogError($"Error while creating user - form is invalid");
                    return Enums.Result.Error;
                }
                var now = DateTime.UtcNow;
                ApplicationUser newUser = new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    Email = req.Email.ToLower(),
                    NormalizedEmail = req.Email.Normalize().ToUpper(),
                    UserName = req.Email.ToLower(),
                    NormalizedUserName = req.Email.Normalize().ToUpper(),
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    CreatedAt = now,
                    LastLogin = now,
                    IsActive = true,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserSettings = new ApplicationUserSettings
                    {
                        Language = user.UserSettings?.Language ?? Shared.Models.Enums.Language.Polish,
                        ThemeMode = user.UserSettings?.ThemeMode ?? Shared.Models.Enums.ThemeMode.LightMode,
                        DecimalSeparator = user.UserSettings?.DecimalSeparator ?? Shared.Models.Enums.DecimalSeparator.Comma,
                        DateFormat = user.UserSettings?.DateFormat ?? Shared.Models.Enums.DateFormat.DD_MM_RRRR_DOTS,
                        TimeFormat = user.UserSettings?.TimeFormat ?? Shared.Models.Enums.TimeFormat.HH_MM_24H,
                        TimeZone = user.UserSettings?.TimeZone ?? Shared.Models.Enums.TimeZone.Europe_Warsaw
                    }
                };
                var password = new PasswordHasher<ApplicationUser>();
                newUser.PasswordHash = password.HashPassword(newUser, req.Password);

                var result = await repo.AddAsync(newUser);
                if(result != Enums.Result.Success)
                {
                    logger.LogWarning($"Error while creating user");
                    return Enums.Result.Error;
                }
                logger.LogInformation($"User {user.Email} created user {newUser.Email}");
                return Enums.Result.Success;
            });

            authGroup.MapPost("/Logout", async (
               ClaimsPrincipal user,
               [FromServices] SignInManager<ApplicationUser> signInManager,
               [FromForm] string returnUrl,
               IMemoryCache cache) =>
            {
                var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value?.ToString();
                if (!string.IsNullOrEmpty(userId))
                {
                    var cacheKey = $"user_permissions_{userId}";
                    if (cache.TryGetValue(cacheKey, out var value))
                        cache.Remove(cacheKey);
                }
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect($"~/{returnUrl}");
            });

            return authGroup;
        }
    }
}
