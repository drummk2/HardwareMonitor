using HardwareMonitor.ServiceClasses;

namespace HardwareMonitor.ServiceInterfaces
{
    /// <summary>
    /// Defines a contract implemented by <see cref="HardwareMonitoringService"/> class.
    /// </summary>
    public interface IHardwareMonitoringService
    {
        /// <summary>
        /// Start the service when prompted (called by the TopShelf host factory).
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the service when prompted (called by the TopShelf host factory).
        /// </summary>
        void Stop();
    }
}
