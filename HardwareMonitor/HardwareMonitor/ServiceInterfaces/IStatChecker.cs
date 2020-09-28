using HardwareMonitor.ServiceClasses;
using System.Threading.Tasks;

namespace HardwareMonitor.ServiceInterfaces
{
    /// <summary>
    /// Defines a contract implemented by <see cref="StatChecker"/> class.
    /// </summary>
    public interface IStatChecker
    {
        Task LogCurrentStatistics(); 
    }
}