using MagFlow.Web.HealthChecks;
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
