using System.Threading;
using System.Threading.Tasks;
using WinServiceBaseCore.Framework.Logging;

namespace WinServiceBaseCore.Framework
{
    public interface IProcessBase
    {
        string StopCode
        {
            get;
        }

        string ExitInstructions
        {
            get;
        }

        bool CanStartProcess
        {
            get;
        }

        /// <summary>
        /// Returns the name of the current process for use in error messages etc...
        /// </summary>
        string ProcessName
        {
            get;
        }

        /// <summary>
        /// Provides access to a logger for the process
        /// </summary>
        ILogger ProcessLogger
        {
            get;
        }

        /// <summary>
        /// Starting point for the logic of the process
        /// </summary>
        void ExecuteProcess();

        /// <summary>
        /// Process Logic
        /// </summary>
        void DoProcessWork();

        /// <summary>
        /// Instantiates Thread to call ExecuteProcess on start; also instantiates the exit event for the thread
        /// <para>Any necessary setup prior to the start of thread should be done in an override that calls 
        /// base.Start() at the end</para>
        /// </summary>
        Task StartAsync( CancellationToken cancellationToken );

        /// <summary>
        /// Base version of stop provides basic implementation to stop any instantiated threads gracefully
        /// and log any errors.
        /// </summary>
        Task StopAsync( CancellationToken cancellationToken );
    }
}
