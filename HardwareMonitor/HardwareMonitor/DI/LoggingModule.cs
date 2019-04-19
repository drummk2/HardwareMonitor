using Autofac;
using Autofac.Core;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HardwareMonitor
{
    /// <summary>
    /// Initialises the logger for the Hardware Monitor.
    /// </summary>
    public class LoggingModule : Autofac.Module
    {
        private static void InjectLoggerProperties(object instance)
        {
            Type instanceType = instance.GetType();

            IEnumerable<PropertyInfo> properties = instanceType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(ILog) 
                    && p.CanWrite 
                    && p.GetIndexParameters().Length == 0);

            foreach (PropertyInfo propToSet in properties)
                propToSet.SetValue(instance, LogManager.GetLogger(instanceType), null);
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e) => e.Parameters = e.Parameters.Union(
                new[]
                {
                    new ResolvedParameter(
                        (p, i) => p.ParameterType == typeof(ILog),
                        (p, i) => LogManager.GetLogger(p.Member.DeclaringType)
                    ),
                });

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }
    }
}
