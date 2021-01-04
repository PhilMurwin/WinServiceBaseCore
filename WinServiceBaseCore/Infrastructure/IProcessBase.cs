using Microsoft.Extensions.Hosting;

namespace WinServiceBaseCore.Infrastructure
{
    public interface IProcessBase : IHostedService
    {
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
