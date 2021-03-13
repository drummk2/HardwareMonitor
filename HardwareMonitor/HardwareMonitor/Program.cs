using Autofac;
using HardwareMonitor.ServiceInterfaces;
using log4net.Config;
using Topshelf;

namespace HardwareMonitor
{
    /// <summary>
    /// Configures and initialises the Hardware Monitoring Service.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Initialise the TopShelf service factory and set any relevant properties.
        /// </summary>
        private static void Main()
        {
            XmlConfigurator.Configure();
            HostFactory.Run(factory =>
            {
                factory.Service<IHardwareMonitoringService>(service =>
                {
                    service.ConstructUsing(s => BuildContainer().Resolve<IHardwareMonitoringService>());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                factory.RunAsLocalSystem();
                factory.StartAutomatically();
                factory.SetDisplayName("Hardware Monitor");
                factory.SetServiceName("Hardware Monitor");
            });
        }

        /// <summary>
        /// Build an AutoFac container to be used by the service.
        /// </summary>
        /// <returns>A pre-built AutoFac container.</returns>
        private static IContainer BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<LoggingModule>()
            builder.RegisterModule<StandardModule>();
            return builder.Build();
        }
    }
}