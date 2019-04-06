﻿using Autofac;
using HardwareMonitor.ServiceClasses;
using HardwareMonitor.ServiceInterfaces;

namespace HardwareMonitor.DI
{
    /// <summary>
    /// Initialises all classes for the Hardware Monitor (excluding the Logger).
    /// </summary>
    public class StandardModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HardwareMonitoringService>()
                   .As<IHardwareMonitoringService>();
            builder.RegisterType<HardwareStatChecker>()
                   .As<IHardwareStatChecker>();
        }
    }
}
