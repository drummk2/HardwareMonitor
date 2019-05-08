using System.Threading.Tasks;

namespace HardwareMonitor.ServiceInterfaces
{
    /// <summary>
    /// Hardware Stat Checker contract.
    /// </summary>
    public interface IStatChecker
    {
        Task LogCurrentStatistics(); 
    }
}
