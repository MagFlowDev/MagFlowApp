using Serilog;

namespace MagFlow.Web.Extensions
{
    public static class LoggingExtensions
    {
        public static WebApplicationBuilder AddMagFlowLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            builder.Host.UseSerilog((ctx, sp, cfg) =>
            {
                var root = AppContext.BaseDirectory;
                var logsDir = Path.Combine(root, "logs");
                Directory.CreateDirectory(logsDir);

                cfg.ReadFrom.Configuration(ctx.Configuration)
                   .ReadFrom.Services(sp)
                   .Enrich.FromLogContext()
                   .Enrich.WithThreadId()
                   .Enrich.WithEnvironmentName()
                   .WriteTo.Console()
                   .WriteTo.File(
                        path: Path.Combine(logsDir, "magflow-.log"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14,
                        shared: true,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] ({EnvironmentName}/{ThreadId}) {SourceContext}: {Message:lj}{NewLine}{Exception}"
                    );
            });

            return builder;
        }
    }
}
