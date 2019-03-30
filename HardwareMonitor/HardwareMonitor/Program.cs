using Castle.Windsor;
using HardwareMonitor.DI;
using HardwareMonitor.ServiceInterfaces;
using Topshelf;

namespace HardwareMonitor
{
    /// <summary>
    /// Configured and initialises the Hardware Monitoring Service.
    /// </summary>
    public class Program
    {
        public static void Main() =>
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

        private static IWindsorContainer BuildContainer() => new WindsorContainer().Install(new WindsorInstaller());
    }
}