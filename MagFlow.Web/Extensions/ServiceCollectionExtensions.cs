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

            return services;
        }
    }
}
