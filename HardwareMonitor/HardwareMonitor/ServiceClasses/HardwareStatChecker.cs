using HardwareMonitor.ServiceInterfaces;
using log4net;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HardwareMonitor.ServiceClasses
{
    public class HardwareStatChecker : IHardwareStatChecker
    {
        private readonly ILog _log;

        public HardwareStatChecker(ILog log) => _log = log;

        private string CheckCurrentCPUUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            cpuCounter.NextValue();
            Task.Delay(500).Wait();
            return $"{Math.Round(cpuCounter.NextValue(), 2)}%";
        }

        private string CheckCurrentAvailableRAM()
        {
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
            ramCounter.NextValue();
            Task.Delay(500).Wait();
            return $"{Math.Round(ramCounter.NextValue() / 1024, 2)}";
        }

        public void LogCurrentStatistics()
        {
            _log.Info($"CPU Usage - {DateTime.Now.ToLocalTime()} {CheckCurrentCPUUsage(), 10}");
            _log.Info($"RAM Usage - {DateTime.Now.ToLocalTime()} {CheckCurrentAvailableRAM(), 10}Gb");
        }
    }
}