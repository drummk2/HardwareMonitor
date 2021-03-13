using HardwareMonitor.ServiceInterfaces;
using log4net;
using System;
using System.Configuration;
using System.Timers;

namespace HardwareMonitor.ServiceClasses
{
    /// <inheritdoc cref="IHardwareMonitoringService"/>
    public class HardwareMonitoringService : IHardwareMonitoringService
    {
        /// <summary>
        /// An instance of the <see cref="StatChecker"/> class.
        /// </summary>
        private readonly IStatChecker _hardwareStatChecker;
        
        /// <summary>
        /// An injected Log4net instance, see <see cref="LoggingModule"/> for futher information.
        /// </summary>
        private readonly ILog _log;
        
        /// <summary>
        /// A timer instance to moderate when the service runs.
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// Initialise a timer to control the rate at which the service executes as well as a statistics checker.
        /// </summary>
        public HardwareMonitoringService(IStatChecker hardwareStatChecker, ILog log)
        {
            _hardwareStatChecker = hardwareStatChecker;
            _log = log;
            _timer = new Timer(double.Parse(ConfigurationManager.AppSettings["ServiceTimerDelayIntervalInSeconds"]) * 1000);
            _timer.AutoReset = true;
            _timer.Elapsed += async (sender, e) => await _hardwareStatChecker.LogCurrentStatistics().ConfigureAwait(false);
            _timer.Enabled = true;
        }

        /// <inheritdoc/>
        public void Start() => _log.Info($"STARTING!{Environment.NewLine}");

        /// <inheritdoc/>
        public void Stop() => _log.Info($"STOPPING!{Environment.NewLine}");
    }
}
