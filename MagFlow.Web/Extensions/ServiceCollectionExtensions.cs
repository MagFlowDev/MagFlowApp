using MagFlow.BLL.Helpers;
using MagFlow.BLL.Services;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories;
using MagFlow.DAL.Repositories.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF;
using MagFlow.Shared.Models.Settings;
using MagFlow.Web.Auth;
using MagFlow.Web.HealthChecks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Syncfusion.Blazor;

namespace MagFlow.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMagFlowServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSyncfusionBlazor();
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddHttpContextAccessor();
            services.RegisterScopes();
            services.ConfigureDbContext();
            services.ConfigureAuthorization();

            services.AddHttpClient();
            services.AddScoped(sp =>
            {
                var navigationManager = sp.GetRequiredService<NavigationManager>();
                var handler = new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = new System.Net.CookieContainer()
                };
                return new HttpClient(handler)
                {
                    BaseAddress = new Uri(navigationManager.BaseUri)
                };
            });

            services.AddMagFlowHealthChecks();
            return services;
        }

        private static IServiceCollection AddMagFlowHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" })
                .AddCheck<SqlServerPingHealthCheck>("sql", tags: new[] { "ready" });

            return services;
        }

        private static void ConfigureDbContext(this IServiceCollection services)
        {
            services.AddDbContextFactory<CoreDbContext, CoreDbContextFactory>();
            services.AddDbContextFactory<CompanyDbContext, CompanyDbContextFactory>();
            services.AddScoped<ICoreDbContextFactory, CoreDbContextFactory>();
            services.AddScoped<ICompanyDbContextFactory, CompanyDbContextFactory>();
        }

        private static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Stores.MaxLengthForKeys = 128;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<CoreDbContext>()
            .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/Denied";

                options.Cookie.MaxAge = TimeSpan.FromHours(12);
                options.SlidingExpiration = true;

                options.Cookie.Name = "MagFlow.Auth";
                options.Cookie.Path = "/";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(30);
            });
            services.AddAuthorization();
            services.AddCascadingAuthenticationState();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            services.AddScoped<RoleManager<ApplicationRole>>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            services.AddScoped<IEmailSender<ApplicationUser>>(sp => sp.GetRequiredService<IEmailService>());
            services.AddScoped<IUserStore<ApplicationUser>, FactoryUserStore>();
            services.AddScoped<IRoleStore<ApplicationRole>, FactoryRoleStore>();
        }

        private static void RegisterScopes(this IServiceCollection services)
        {
            services.RegisterRepositories();
            services.RegisterServices();

        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
