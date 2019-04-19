using Autofac;
using HardwareMonitor.ServiceInterfaces;
using log4net.Config;
using Topshelf;

namespace HardwareMonitor
{
    /// <summary>
    /// Configures and initialises the Hardware Monitoring Service.
    /// </summary>
    public class Program
    {
        public static void Main()
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
                factory.SetDisplayName("Hardware Monitoring Service");
                factory.SetServiceName("Hardware Monitoring Service");
            });
        }

        private static IContainer BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<StandardModule>();
            builder.RegisterModule<LoggingModule>();
            return builder.Build();
        }
    }
}