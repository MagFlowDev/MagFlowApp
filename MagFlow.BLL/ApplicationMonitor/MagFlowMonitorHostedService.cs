using Autofac;
using MagFlow.BLL.Helpers;
using MagFlow.BLL.Services.Heartbeat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.ApplicationMonitor
{
    public class MagFlowMonitorHostedService : IHostedService
    {
        private readonly IApplicationMonitorService _applicationMonitorService;
        private readonly IContainerBuilder _containerBuilder;
        private readonly ILogger<MagFlowMonitorHostedService> _logger;

        public MagFlowMonitorHostedService(
            ILogger<MagFlowMonitorHostedService> logger,
            IContainerBuilder containerBuilder,
            IApplicationMonitorService applicationMonitorService)
        {
            _applicationMonitorService = applicationMonitorService;
            _logger = logger;
            _containerBuilder = containerBuilder; 
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("====================================================================================================");
            _logger.LogInformation("Starting MagFlowMonitor");

            _applicationMonitorService.StartHeartbeat();
            RegisterServices();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _applicationMonitorService.StopHeartbeat();
            return Task.CompletedTask;
        }

        private void RegisterServices()
        {
            if (_containerBuilder == null)
                return;

            _containerBuilder.LifetimeScope = _containerBuilder.RootScope.BeginLifetimeScope(scope =>
            {
            });
            _containerBuilder.LifetimeScope.Resolve<ApplicationMessageInfoService>();
        }
    }
}
