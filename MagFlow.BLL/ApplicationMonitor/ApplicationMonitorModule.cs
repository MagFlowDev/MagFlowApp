using Autofac;
using MagFlow.BLL.Helpers;
using MagFlow.BLL.Services.Heartbeat;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.BLL.Services.Notifications;
using MagFlow.DAL.Repositories.Core;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.EF;
using MagFlow.EF.MultiTenancy;
using MagFlow.Shared.Models;
using Microsoft.AspNetCore.Http;
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

        protected override void Load(Autofac.ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new ApplicationStateInfo()).SingleInstance()
                .OnActivated(x => x.Instance.OverallState = Enums.OverallState.Inoperative);

            builder.RegisterType<ApplicationMonitorService>().AsImplementedInterfaces()
                .WithParameter((i, c) => i.ParameterType == typeof(int), (i, c) => HeartbeatInterval)
                .WithParameter((i, c) => i.ParameterType == typeof(TimeSpan), (i, c) => HeartbeatTimeout)
                .SingleInstance();

            builder.RegisterType<ModuleHeartbeatService>().As<IModuleHeartbeatService>()
                .WithParameter((i, c) => i.Name == "heartbeatInterval", (i, c) => HeartbeatInterval)
                .SingleInstance();

            builder.RegisterType<MagFlowMonitorHostedService>().AsSelf().SingleInstance();

            builder.RegisterType<TenantProvider>().As<ITenantProvider>().SingleInstance();
            builder.RegisterType<CompanyContext>().As<ICompanyContext>().SingleInstance();
            builder.RegisterType<CoreDbContextFactory>().As<ICoreDbContextFactory>().SingleInstance();
            builder.RegisterType<CompanyDbContextFactory>().As<ICompanyDbContextFactory>().SingleInstance();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            RegisterRepositories(builder);
            RegisterServices(builder);
        }

        private void RegisterRepositories(Autofac.ContainerBuilder builder)
        {
            builder.RegisterType<NotificationRepository>().As<INotificationRepository>().SingleInstance();
        }

        private void RegisterServices(Autofac.ContainerBuilder builder)
        {
            builder.RegisterType<NotificationService>().As<INotificationService>().SingleInstance();
        }
    }
}
