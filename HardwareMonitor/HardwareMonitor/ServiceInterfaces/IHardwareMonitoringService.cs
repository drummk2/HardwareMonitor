using HardwareMonitor.ServiceClasses;

namespace HardwareMonitor.ServiceInterfaces
{
    /// <summary>
    /// Defines a contract implemented by <see cref="HardwareMonitoringService"/> class.
    /// </summary>
    public interface IHardwareMonitoringService
    {
        void Start();

        void Stop();
    }
}
