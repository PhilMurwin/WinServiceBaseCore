using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinServiceBaseCore.Infrastructure
{
    /// <summary>
    /// Base class for creating processes for the service to execute
    /// </summary>
    public abstract class ProcessBase : BackgroundService, IProcessBase
    {
        protected const int DefaultSleepSeconds = 30 * 1000;
        protected const int MillisecondsInMinute = 60 * 1000;

        private ILogger _logger;

        /// <summary>
        /// Provides access to a logger for the process
        /// </summary>
        public ILogger ProcessLogger
        {
            get { return _logger ?? (_logger = NLog.LogManager.GetCurrentClassLogger()); }
        }

        /// <summary>
        /// Returns the name of the current process for use in error messages etc...
        /// </summary>
        public string ProcessName
        {
            get { return GetType().Name; }
        }

        public abstract string StopCode
        {
            get;
        }

        public string ExitInstructions
        {
            get { return string.Format( "{0} Process started. Type '{1}' to stop the process.", ProcessName, StopCode ); }
        }

        public abstract bool CanStartProcess
        {
            get;
        }

        /// <summary>
        /// Frequncy (in minutes) to execute the process.
        /// <para>This should typically be set via a config setting that is read in the override of this property.</para>
        /// </summary>
        public abstract int Frequency
        {
            get;
        }

        /// <summary>
        /// This time is used to determine how often (the frequency) DoProcessWork is executed.
        /// <para>It defaults to the current time on initialization.</para>
        /// </summary>
        protected DateTime LastRunTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Default Process Loop
        /// <para>Sleeps the thread/process for some amount of time after each run of DoProcessWork.</para>
        /// <para>Override this if you need a less frequent process run frequency.</para>
        /// <para>If you override this method don't call the base version and be sure to set the LastRunTime after your process executes</para>
        /// </summary>
        protected override async Task ExecuteAsync( CancellationToken stoppingToken )
        {
            // Set LastRunTime to now - frequency so the process executes immediately on startup
            LastRunTime = DateTime.Now.AddMinutes( -Frequency );

            // If cancellation signaled break out of loop
            while ( !stoppingToken.IsCancellationRequested )
            {
                if ( DateTime.Now >= LastRunTime.AddMinutes( Frequency ) )
                {
                    ProcessLogger.Debug( "Calling DoProcessWork for process [{0}].", ProcessName );

                    DoProcessWork();

                    LastRunTime = DateTime.Now;
                }

                // Wait n milliseconds for exit event signal before continuing
                await Task.Delay( DefaultSleepSeconds, stoppingToken );
            }
        }

        /// <summary>
        /// Process Logic
        /// </summary>
        public abstract void DoProcessWork();
    }
}
