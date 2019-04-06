using HardwareMonitor.ServiceInterfaces;
using log4net;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HardwareMonitor.ServiceClasses
{
    /// <summary>
    /// Retrieves statistics about the user's PC. For example, the CPU or available RAM.
    /// </summary>
    public class HardwareStatChecker : IHardwareStatChecker
    {
        private readonly ILog _log;

        public HardwareStatChecker(ILog log) => _log = log;

        /// <summary>
        /// Check the PC's current CPU usage and log said value.
        /// </summary>
        /// <returns></returns>
        private string CheckCurrentCPUUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            cpuCounter.NextValue();
            Task.Delay(500).Wait();
            return $"{Math.Round(cpuCounter.NextValue(), 2)}%";
        }

        /// <summary>
        /// Check the PC's current available RAM and log said value.
        /// </summary>
        /// <returns></returns>
        private string CheckCurrentAvailableRAM()
        {
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
            ramCounter.NextValue();
            Task.Delay(500).Wait();
            return $"{Math.Round(ramCounter.NextValue() / 1024, 2)}";
        }

        /// <summary>
        /// Log any relevant statistics for the user's PC.
        /// </summary>
        public void LogCurrentStatistics()
        {
            _log.Info($"CPU Usage - {DateTime.Now.ToLocalTime()} {CheckCurrentCPUUsage(), 10}");
            _log.Info($"RAM Usage - {DateTime.Now.ToLocalTime()} {CheckCurrentAvailableRAM(), 10}Gb");
        }
    }
}