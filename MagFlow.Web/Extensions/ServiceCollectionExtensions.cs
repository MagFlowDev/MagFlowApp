using MagFlow.BLL.Services;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories;
using MagFlow.DAL.Repositories.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.EF;
using MagFlow.Shared.Models.Settings;
using MagFlow.Web.HealthChecks;
using Microsoft.AspNetCore.Identity;
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

            services.RegisterScopes();
            services.ConfigureDbContext();

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
        }

        private static void RegisterScopes(this IServiceCollection services)
        {
            services.RegisterRepositories();
            services.RegisterServices();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
