namespace HardwareMonitor.ServiceInterfaces
{
    /// <summary>
    /// Hardware Monitoring Service contract.
    /// </summary>
    public interface IHardwareMonitoringService
    {
        void Start();

        void Stop();
    }
}
