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
    /// <inheritdoc cref="IStatChecker"/>
    public class StatChecker : IStatChecker
    {
        /// <summary>
        /// An injected Log4net instance, see <see cref="LoggingModule"/> for futher information.
        /// </summary>
        private readonly ILog _log;

        /// <summary>
        /// Initialise an instance of the <see cref="StatChecker"/> class.
        /// </summary>
        /// <param name="log"></param>
        public StatChecker(ILog log) => _log = log;

        /// <summary>
        /// Checks the current free space on all "Fixed" drives on the user's machine.
        /// </summary>
        private async Task CheckAllDrives()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives().Where(d => Equals(d.DriveType, DriveType.Fixed)))
                _log.Info($"Free Space ({drive.Name}) -> {await CheckCurrentDriveSpace(drive.Name).ConfigureAwait(false)}Gb");
        }

        /// <summary>
        /// Checks the current free space on a specified drive.
        /// </summary>
        /// <param name="driveName">The name of the drive in question.</param>
        /// <returns>The free space on the specified drive.</returns>
        private Task<long> CheckCurrentDriveSpace(string driveName)
            => Task.FromResult(
                   DriveInfo.GetDrives()
                       .Where(d => Equals(d.Name, driveName))
                       .Select(d => d.AvailableFreeSpace)
                       .FirstOrDefault() / (1024 * 1024 * 1024));

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
                DateTime startTime = DateTime.Now;
                client.DownloadFile("http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.js", "./jquery.js");
                DateTime endTime = DateTime.Now;
                downloadSpeeds[i] = Math.Round(261 / (endTime - startTime).TotalSeconds);
            }
            return Task.FromResult($"{Math.Round(downloadSpeeds.Average() / 1024, 2)}");
        }

        /// <inheritdoc/>
        public async Task LogCurrentStatistics()
        {
            try
            {
                await CheckAllDrives().ConfigureAwait(false);
                _log.Info($"CPU Usage        -> {await CheckCurrentCPUUsage().ConfigureAwait(false)}");
                _log.Info($"Available RAM    -> {await CheckCurrentAvailableRAM().ConfigureAwait(false)}Gb");
                _log.Info($"Internet Speed   -> {await CheckCurrentDownloadSpeed().ConfigureAwait(false)}Mb/s{Environment.NewLine}");
            }
            catch (Exception e)
            {
                _log.Error("An error has occurred in the Hardware Monitoring Service", e);
            }
        }
    }
}
