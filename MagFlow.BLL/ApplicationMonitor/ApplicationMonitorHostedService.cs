using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public ApplicationMonitorHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var hostBuilder = new HostBuilder()
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureContainer<ContainerBuilder>(builder =>
                    {
                        builder.RegisterModule(new ApplicationMonitorModule());
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
    }
}
