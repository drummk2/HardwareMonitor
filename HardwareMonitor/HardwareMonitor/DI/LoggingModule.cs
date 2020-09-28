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
        /// <summary>
        /// Handles any property injection of type ILog in the component in question.
        /// </summary>
        /// <param name="instance">The class instance that contains the property injection in question.</param>
        private static void InjectLoggerProperties(object instance)
        {
            Type instanceType = instance.GetType();

            IEnumerable<PropertyInfo> properties = instanceType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(ILog)
                    && p.CanWrite
                    && p.GetIndexParameters().Length == 0);

            properties.ToList().ForEach(p => p.SetValue(instance, LogManager.GetLogger(instanceType), null));
        }

        /// <summary>
        /// Handles any constructor injection of type ILog in the component in question.
        /// </summary>
        private static void OnComponentPreparing(object sender, PreparingEventArgs e) => e.Parameters = e.Parameters.Union(
            new[] {
                new ResolvedParameter(
                    (p, i) => p.ParameterType == typeof(ILog),
                    (p, i) => LogManager.GetLogger(p.Member.DeclaringType)
                ),
            });

        /// <summary>
        /// Override the Preparing and Activated events on IComponentRegistration to allow the injection of an ILog via constructor or property.
        /// </summary>
        /// <param name="componentRegistry">An AutoFac registry containing all component registrations for the application.</param>
        /// <param name="registration">An AutoFac component registration to attach functionality to.</param>
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }
    }
}