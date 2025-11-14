using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Helpers
{
    public interface IContainerBuilder
    {
        public string ContainerName { get; set; }
        public ILifetimeScope RootScope { get; set; }
        public ILifetimeScope LifetimeScope { get; set; }
    }

    public class ContainerBuilder : IContainerBuilder
    {
        public string ContainerName { get; set; }
        public ILifetimeScope RootScope { get; set; }
        public ILifetimeScope LifetimeScope { get; set; }
    }
}
