using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WinServiceBaseCore.Infrastructure
{
    public interface IProcessBase<T> : IHostedService where T: IProcessBase<T>
    {
        /// <summary>
        /// Provides access to a logger for the process
        /// </summary>
        ILogger<T> ProcessLogger { get; set; }

        string StopCode { get; }

        string ExitInstructions { get; }

        bool CanStartProcess { get; }

        /// <summary>
        /// Returns the name of the current process for use in error messages etc...
        /// </summary>
        string ProcessName { get; }

        int Frequency { get; }

        /// <summary>
        /// Process Logic
        /// </summary>
        void DoProcessWork();
    }
}
