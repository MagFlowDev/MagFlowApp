using MagFlow.BLL.ApplicationMonitor;
using MagFlow.BLL.Helpers;
using MagFlow.BLL.Helpers.Auth;
using MagFlow.BLL.Services;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories;
using MagFlow.DAL.Repositories.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF;
using MagFlow.EF.MultiTenancy;
using MagFlow.Shared.Models.Settings;
using MagFlow.Web.Auth;
using MagFlow.Web.HealthChecks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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

            services.ConfigureOpenTelemetry();
            services.AddHttpContextAccessor();

            services.AddHostedService<ApplicationMonitorHostedService>();

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
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddScoped<ICompanyContext, CompanyContext>();
            services.AddScoped<ICoreDbContextFactory, CoreDbContextFactory>();
            services.AddScoped<ICompanyDbContextFactory, CompanyDbContextFactory>();
            services.AddDbContextFactory<CoreDbContext, CoreDbContextFactory>();
            //services.AddDbContextFactory<CompanyDbContext, CompanyDbContextFactory>();
        }

        private static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddCascadingAuthenticationState();
            services.AddScoped<IdentityUserAccessor>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<RoleManager<ApplicationRole>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            services.AddScoped<IEmailSender<ApplicationUser>>(sp => sp.GetRequiredService<IEmailService>());
            services.AddScoped<IUserStore<ApplicationUser>, FactoryUserStore>();
            services.AddScoped<IRoleStore<ApplicationRole>, FactoryRoleStore>();

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
                    options.Lockout = new LockoutOptions()
                    {
                        MaxFailedAccessAttempts = 10,
                        DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1)
                    };
                })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<CoreDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager()
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
        }

        private static void RegisterScopes(this IServiceCollection services)
        {
            services.RegisterRepositories();
            services.RegisterServices();

        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }

        private static void ConfigureOpenTelemetry(this IServiceCollection services)
        {
            if (AppSettings.OtelSettings == null || !AppSettings.OtelSettings.Enabled || string.IsNullOrEmpty(AppSettings.OtelSettings.Address))
                return;
            services.AddOpenTelemetry()
               .WithTracing(tracing =>
               {
                   tracing
                       .AddSource("MagFlowActivitySource")
                       .AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .SetResourceBuilder(ResourceBuilder.CreateDefault()
                           .AddService("MagFlow")
                           .AddTelemetrySdk())
                       .AddOtlpExporter(options => options.Endpoint = new Uri(AppSettings.OtelSettings.Address));
               })
               .WithMetrics(metrics =>
               {
                   metrics
                       .AddMeter("MagFlowMetrics")
                       .AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .SetResourceBuilder(ResourceBuilder.CreateDefault()
                           .AddService("MagFlow")
                           .AddTelemetrySdk())
                       .AddOtlpExporter(options => options.Endpoint = new Uri(AppSettings.OtelSettings.Address));
               });
        }
    }
}
