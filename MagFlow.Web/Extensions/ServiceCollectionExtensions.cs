using MagFlow.Domain.Core;
using MagFlow.EF;
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

            string coreConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Core;User Id=sa;Password=Password1!;TrustServerCertificate=True";
            string companyConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Company;User Id=sa;Password=Password1!;TrustServerCertificate=True";

            services.AddDbContext<CoreDbContext>(options =>
                options.UseSqlServer(coreConnectionString));
            services.AddDbContext<CompanyDbContext>(options =>
                options.UseSqlServer(companyConnectionString));

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(o => { });
            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.Stores.MaxLengthForKeys = 128;
                o.SignIn.RequireConfirmedEmail = true;
            })
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
    }
}
