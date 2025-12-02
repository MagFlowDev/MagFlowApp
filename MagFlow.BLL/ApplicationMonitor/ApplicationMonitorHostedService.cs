using Autofac;
using Autofac.Extensions.DependencyInjection;
using MagFlow.BLL.Extensions;
using MagFlow.BLL.Helpers;
using MagFlow.BLL.Services.Heartbeat;
using MagFlow.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagFlow.BLL.ApplicationMonitor
{
    public class ApplicationMonitorHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationMonitorHostedService> _logger;

        public ApplicationMonitorHostedService(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            ILogger<ApplicationMonitorHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var hostBuilder = new HostBuilder()
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureLogging(loggingBuilder => loggingBuilder.AddModuleLogging(_serviceProvider,_configuration,"monitor"))
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSignalR();
                        services.AddSingleton(sp => _serviceProvider.GetRequiredService<IServerNotificationService>());
                        RegisterHeartbeatServices(services);
                        services.AddHostedService<MagFlowMonitorHostedService>();
                    })
                    .ConfigureContainer<Autofac.ContainerBuilder>(builder =>
                    {
                        builder.RegisterModule(new ApplicationMonitorModule());

                        var containerBuilder = new Helpers.ContainerBuilder();
                        builder.RegisterBuildCallback(rootScope =>
                        {
                            containerBuilder.RootScope = rootScope;
                        });

                        builder.Register(_ => containerBuilder).As<IContainerBuilder>().SingleInstance();
                    });
                try
                {
                    await hostBuilder.RunConsoleAsync(stoppingToken);
                }
                catch(Exception)
                {

                }
                await Task.Delay(1000 * 60);
            }
        }

        private IServiceCollection RegisterHeartbeatServices(IServiceCollection services)
        {
            services.AddScoped<ApplicationMessageInfoService>();

            return services;
        }
    }
}
