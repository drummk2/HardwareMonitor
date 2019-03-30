using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HardwareMonitor.ServiceClasses;
using HardwareMonitor.ServiceInterfaces;

namespace HardwareMonitor.DI
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store) =>
            container.Register(
                Component.For<IHardwareMonitoringService>()
                         .ImplementedBy<HardwareMonitoringService>()
                         .LifeStyle.Singleton
            );
    }
}
