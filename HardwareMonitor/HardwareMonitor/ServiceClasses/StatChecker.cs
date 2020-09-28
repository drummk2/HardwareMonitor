using HardwareMonitor.ServiceInterfaces;
using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HardwareMonitor.ServiceClasses
{
    /// <summary>
    /// Retrieves statistics about the user's PC. For example, the current CPU usage or available RAM.
    /// </summary>
    public class StatChecker : IStatChecker
    {
        private readonly ILog _log;

        public StatChecker(ILog log) => _log = log;

        /// <summary>
        /// Check the PC's current CPU usage and log said value.
        /// </summary>
        /// <returns>The PC's current CPU usage.</returns>
        private Task<string> CheckCurrentCPUUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            cpuCounter.NextValue();
            Task.Delay(500).Wait();
            return Task.FromResult($"{Math.Round(cpuCounter.NextValue(), 2)}%");
        }

        /// <summary>
        /// Check the PC's current available RAM and log said value.
        /// </summary>
        /// <returns>The current available RAM for the PC.</returns>
        private Task<string> CheckCurrentAvailableRAM()
        {
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
            ramCounter.NextValue();
            Task.Delay(500).Wait();
            return Task.FromResult($"{Math.Round((ramCounter.NextValue() / 1024), 2)}");
        }

        /// <summary>
        /// Check the current download speed for the PC and log said value.
        /// </summary>
        /// <returns>The current download speed for the PC.</returns>
        private Task<string> CheckCurrentDownloadSpeed()
        {
            double[] downloadSpeeds = new double[5];
            for (int i = 0; i < 5; i++)
            {
                WebClient client = new WebClient();
                int jQueryFileSize = 261;
                DateTime startTime = DateTime.Now;
                client.DownloadFile("http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.js", "./jquery.js");
                DateTime endTime = DateTime.Now;
                downloadSpeeds[i] = Math.Round(jQueryFileSize / (endTime - startTime).TotalSeconds);
            }
            return Task.FromResult($"{Math.Round(downloadSpeeds.Average() / 1024, 2)}");
        }

        /// <summary>
        /// Checks the current free space on a specified drive.
        /// </summary>
        /// <param name="driveName">The name of the drive in question.</param>
        /// <returns>The free space on the specified drive.</returns>
        private Task<long> CheckCurrentDriveSpace(string driveName)
        {
            return Task.FromResult(
                DriveInfo.GetDrives()
                    .Where(d => Equals(d.Name, driveName))
                    .Select(d => d.AvailableFreeSpace)
                    .FirstOrDefault() / (1024 * 1024 * 1024));
        }

        /// <summary>
        /// Log any relevant statistics for the user's PC.
        /// </summary>
        public async Task LogCurrentStatistics()
        {
            _log.Info($"CPU Usage        -> {await CheckCurrentCPUUsage().ConfigureAwait(false)}");
            _log.Info($"Available RAM    -> {await CheckCurrentAvailableRAM().ConfigureAwait(false)}Gb");
            _log.Info($"Internet Speed   -> {await CheckCurrentDownloadSpeed().ConfigureAwait(false)}Mb/s");
            _log.Info($"Free Space (C:/) -> {await CheckCurrentDriveSpace(@"C:\").ConfigureAwait(false)}Gb{Environment.NewLine}");
        }
    }
}