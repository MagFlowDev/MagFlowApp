using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System.Runtime.InteropServices;

namespace MagFlow.BLL.Helpers
{
    public static class LoggingBuilder
    {
        private static string OutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] ({EnvironmentName}/{ThreadId}) {SourceContext}: {Message:lj}{NewLine}{Exception}";

        public static WebApplicationBuilder AddMagFlowLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            builder.Host.UseSerilog((ctx, sp, cfg) =>
            {
                var root = AppDomain.CurrentDomain.BaseDirectory;
                var logsDir = Path.Combine(root, "logs", "magflow");
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
                        outputTemplate: OutputTemplate
                    );
            });

            return builder;
        }

        public static ILoggingBuilder AddModuleLogging(this ILoggingBuilder builder, IServiceProvider serviceProvider, IConfiguration configuration, string moduleName)
        {
            builder.AddSerilog(CreateLoggerConfiguration(moduleName, serviceProvider, configuration).CreateLogger());
            return builder;
        }

        public static ILoggingBuilder AddModuleLogging<T>(this ILoggingBuilder builder, IServiceProvider serviceProvider, IConfiguration configuration, string moduleName)
        {
            AddLoggerProvider(builder.Services, serviceProvider, configuration, moduleName, CreateLoggerProvider<T>);
            return builder;
        }

        private static void AddLoggerProvider(IServiceCollection services, IServiceProvider sp, IConfiguration config, string moduleName, Func<string, IServiceProvider, IConfiguration, SerilogLoggerProvider> factory)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, SerilogLoggerProvider>(serviceProvider => factory(moduleName, sp, config)));
        }

        private static SerilogLoggerProvider CreateLoggerProvider<T>(string moduleName, IServiceProvider sp, IConfiguration config)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var logsDir = Path.Combine(root, "logs", moduleName);
            Directory.CreateDirectory(logsDir);

            var serilog = Log.Logger ?? CreateLoggerConfiguration(moduleName, sp, config).CreateLogger();
            ILoggerFactory factory = new LoggerFactory().AddSerilog(serilog);
            SerilogLoggerProvider loggerProvider = new SerilogLoggerProvider((Serilog.ILogger?)factory.CreateLogger<T>());
            return loggerProvider;
        }


        public static LoggerConfiguration CreateLoggerConfiguration(string moduleName, IServiceProvider sp, IConfiguration config)
        {
            var osModuleName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? moduleName.ToLowerInvariant() : moduleName;
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var logsDir = Path.Combine(root, "logs", moduleName);
            Directory.CreateDirectory(logsDir);

            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                   .ReadFrom.Services(sp)
                   .Enrich.FromLogContext()
                   .Enrich.WithThreadId()
                   .Enrich.WithEnvironmentName()
                   .WriteTo.Console()
                   .WriteTo.File(
                        path: Path.Combine(logsDir, $"{moduleName}-.log"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14,
                        shared: true,
                        outputTemplate: OutputTemplate);
            return loggerConfiguration;
        }
    }

}
