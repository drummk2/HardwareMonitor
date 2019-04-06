﻿using HardwareMonitor.ServiceInterfaces;
using log4net;
using System.Configuration;
using System.Timers;

namespace HardwareMonitor.ServiceClasses
{
    /// <summary>
    /// Periodically monitors the user's PC and logs performance statistics for that time.
    /// </summary>
    public class HardwareMonitoringService : IHardwareMonitoringService
    {
        private IHardwareStatChecker _hardwareStatChecker;
        private readonly ILog _log;
        private readonly Timer _timer;

        /// <summary>
        /// Initialise a timer to control the rate at which the service executes.
        /// </summary>
        public HardwareMonitoringService(IHardwareStatChecker hardwareStatChecker, ILog log)
        {
            _hardwareStatChecker = hardwareStatChecker;
            _log = log;
            _timer = new Timer(double.Parse(ConfigurationManager.AppSettings["ServiceTimerDelayInterval"]));
            _timer.AutoReset = true;
            _timer.Elapsed += (source, eventargs) => { _hardwareStatChecker.LogCurrentStatistics(); };
            _timer.Enabled = true;
        }

        /// <summary>
        /// Start the service when prompted.
        /// </summary>
        public void Start() => _log.Info("STARTING!");

        /// <summary>
        /// Stop the service when prompted.
        /// </summary>
        public void Stop() => _log.Info("STOPPING!");
    }
}