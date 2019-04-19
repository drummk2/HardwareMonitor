using HardwareMonitor.ServiceInterfaces;
using log4net;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HardwareMonitor.ServiceClasses
{
    /// <summary>
    /// Retrieves statistics about the user's PC. For example, the CPU or available RAM.
    /// </summary>
    public class StatChecker : IStatChecker
    {
        private readonly ILog _log;

        public StatChecker(ILog log) => _log = log;

        /// <summary>
        /// Check the PC's current CPU usage and log said value.
        /// </summary>
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
            return $"{Math.Round((ramCounter.NextValue() / 1024) / 1024, 2)}";
        }

        /// <summary>
        /// Check the current download speed for the PC and log said value.
        /// </summary>
        private string CheckCurrentDownloadSpeed()
        {
            double[] downloadSpeeds = new double[5];
            for (int i = 0; i < 5; i++)
            {
                WebClient client = new WebClient();
                int jQueryFileSize = 261;
                DateTime startTime = DateTime.Now;
                client.DownloadFile("http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.js", "./jquery.js");
                DateTime endTime = DateTime.Now;
                downloadSpeeds[i] = Math.Round((jQueryFileSize / (endTime - startTime).TotalSeconds));
            }
            return $"{downloadSpeeds.Average()}";
        }

        /// <summary>
        /// Log any relevant statistics for the user's PC.
        /// </summary>
        public void LogCurrentStatistics()
        {
            _log.Info($"CPU Usage      ->\t{CheckCurrentCPUUsage()}");
            _log.Info($"RAM Usage      ->\t{CheckCurrentAvailableRAM()}Gb");
            _log.Info($"Internet Speed ->\t{CheckCurrentDownloadSpeed()}Mb/s{Environment.NewLine}");
        }
    }
}