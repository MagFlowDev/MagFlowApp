using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.ApplicationMonitor
{
    public class ApplicationMonitorModule : Module
    {
        public int HeartbeatInterval { get; set; } = 60;
        public TimeSpan HeartbeatTimeout { get; set; } = TimeSpan.FromSeconds(60);

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ApplicationMonitorService>().AsImplementedInterfaces()
                .WithParameter((i, c) => i.ParameterType == typeof(int), (i, c) => HeartbeatInterval)
                .WithParameter((i, c) => i.ParameterType == typeof(TimeSpan), (i, c) => HeartbeatTimeout)
                .SingleInstance();

            builder.RegisterType<ModuleHeartbeatService>().As<IModuleHeartbeatService>()
                .WithParameter((i, c) => i.Name == "heartbeatInterval", (i, c) => HeartbeatInterval)
                .SingleInstance();
        }
    }
}
