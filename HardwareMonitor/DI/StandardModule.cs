using Autofac;
using HardwareMonitor.ServiceClasses;
using HardwareMonitor.ServiceInterfaces;

namespace HardwareMonitor
{
    /// <summary>
    /// A module to encapsulate all classes for the Hardware Monitor (excluding the Logger).
    /// </summary>
    public class StandardModule : Module
    {
        /// <summary>
        /// Load all necessary dependencies (service classes, stat checker, etc...).
        /// </summary>
        /// <param name="builder">An instance of the <see cref="ContainerBuilder"/> class to be used to build an <see cref="IContainer"/>.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HardwareMonitoringService>()
                   .As<IHardwareMonitoringService>()
                   .SingleInstance();

            builder.RegisterType<StatChecker>()
                   .As<IStatChecker>()
                   .SingleInstance();
        }
    }
}
