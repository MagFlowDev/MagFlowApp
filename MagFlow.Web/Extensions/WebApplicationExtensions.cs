using MagFlow.BLL.Helpers;
using MagFlow.Domain.Core;
using MagFlow.EF;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MagFlow.Web.Extensions
{
    public static class WebApplicationExtensions
    {
        private const string LICENSE_KEY = "Mzk4NzgxM0AzMzMwMmUzMDJlMzAzYjMzMzAzYmR3dGZsa2IwMUdqWkRpdC9idWZuSnV5VFAwNUZGbStvVEFQc1AxSkxXSjA9;Mzk4NzgxNEAzMzMwMmUzMDJlMzAzYjMzMzAzYkFQSFc5bW95Z2J1amVuSUVHdFJKMmF2bGZmTEpjeUIvRm8ySy9XZG5Tcmc9";

        public static WebApplication UseMagFlowPipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = (ctx, _, ex) =>
                    ctx.Request.Path.StartsWithSegments("/health")
                    ? Serilog.Events.LogEventLevel.Verbose
                    : (ex is null ? Serilog.Events.LogEventLevel.Information : Serilog.Events.LogEventLevel.Error);
            });

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

            var activitySource = new ActivitySource("MyApplicationActivitySource");
            var meter = new Meter("MyApplicationMetrics");
            var requestCounter = meter.CreateCounter<int>("compute_requests");
            var httpClient = new HttpClient();

            app.MapGet("/test", async (ILogger<Program> logger) =>
            {
                requestCounter.Add(1);

                using (var activity = activitySource.StartActivity("Get data"))
                {
                    // Add data the the activity
                    // You can see these data in Zipkin
                    activity?.AddTag("sample", "value");

                    // Http calls are tracked by AddHttpClientInstrumentation
                    var str1 = await httpClient.GetStringAsync("https://example.com");
                    var str2 = await httpClient.GetStringAsync("https://www.meziantou.net");

                    logger.LogInformation("Response1 length: {Length}", str1.Length);
                    logger.LogInformation("Response2 length: {Length}", str2.Length);
                }

                return Results.Ok();
            });

            app.MapMagFlowHealthChecks();
            return app;
        }

        public static async Task InitializeDatabase(this WebApplication app)
        {
            ILogger? logger = null;
            try
            {
                using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var loggerFactory = serviceScope.ServiceProvider.GetService<ILoggerFactory>();
                    if (loggerFactory != null)
                        logger = loggerFactory.CreateLogger($"{nameof(MagFlow)}.{nameof(MagFlow.Web)}.{nameof(MagFlow.Web.Extensions.WebApplicationExtensions)}");
                    var coreDbContextFactory = serviceScope.ServiceProvider.GetService<ICoreDbContextFactory>();
                    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
                    var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                    if (coreDbContextFactory != null && roleManager != null && userManager != null)
                    {
                        using (var coreDbContext = coreDbContextFactory.CreateDbContext())
                        {
                            await DbInitializer.Initialize(coreDbContext, roleManager, userManager, loggerFactory);
                        }
                        roleManager.Dispose();
                        userManager.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.Message);
            }
        }

        private static WebApplication MapMagFlowHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live"),
            });

            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("ready"),
                ResponseWriter = WriteHealthJson
            });

            return app;
        }

        private static Task WriteHealthJson(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                totalMs = report.TotalDuration.TotalMilliseconds,
                results = report.Entries.ToDictionary(
                    k => k.Key,
                    v => new
                    {
                        status = v.Value.Status.ToString(),
                        description = v.Value.Description,
                        ms = v.Value.Duration.TotalMilliseconds,
                        error = v.Value.Exception?.Message
                    })
            });
            return context.Response.WriteAsync(json);
        }

        
    }
}
