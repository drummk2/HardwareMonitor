using HardwareMonitor.ServiceInterfaces;
using System;
using System.Configuration;
using System.Timers;

namespace HardwareMonitor.ServiceClasses
{
    /// <summary>
    /// Periodically monitors the user's PC and logs performance statistics for that time.
    /// </summary>
    public class HardwareMonitoringService : IHardwareMonitoringService
    {
        private readonly Timer _timer;

        /// <summary>
        /// Initialise a timer to control the rate at which the service executes.
        /// </summary>
        public HardwareMonitoringService()
        {
            _timer = new Timer(double.Parse(ConfigurationManager.AppSettings["ServiceTimerDelayInterval"]));
            _timer.AutoReset = true;
            _timer.Elapsed += (source, eventargs) => { Console.WriteLine(DateTime.Now); };
            _timer.Enabled = true;
        }

        /// <summary>
        /// Start the service when prompted.
        /// </summary>
        public void Start()
        {
            Console.WriteLine("STARTING!");
        }

        /// <summary>
        /// Stop the service when prompted.
        /// </summary>
        public void Stop()
        {
            Console.WriteLine("STOPPING!");
        }
    }
}