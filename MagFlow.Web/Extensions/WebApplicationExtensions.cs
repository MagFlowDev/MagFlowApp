using MagFlow.Web.Components;

namespace MagFlow.Web.Extensions
{
    public static class WebApplicationExtensions
    {
        private const string LICENSE_KEY = "Mzk4NzgxM0AzMzMwMmUzMDJlMzAzYjMzMzAzYmR3dGZsa2IwMUdqWkRpdC9idWZuSnV5VFAwNUZGbStvVEFQc1AxSkxXSjA9;Mzk4NzgxNEAzMzMwMmUzMDJlMzAzYjMzMzAzYkFQSFc5bW95Z2J1amVuSUVHdFJKMmF2bGZmTEpjeUIvRm8ySy9XZG5Tcmc9";

        public static WebApplication UseMagFlowPipeline(this WebApplication app)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(LICENSE_KEY);

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            return app;
        }
    }
}
