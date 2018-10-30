using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WinServiceBaseCore.Framework.Logging;

namespace WinServiceBaseCore.Framework
{
    /// <summary>
    /// Base class for creating processes for the service to execute
    /// </summary>
    public abstract class ProcessBase : IHostedService, IProcessBase, IDisposable
    {
        protected const int DefaultSleepSeconds = 30 * 1000;
        protected const int MillisecondsInMinute = 60 * 1000;

        private Timer _timer;
        private Thread _processThread;
        private AutoResetEvent _threadExitEvent;
        private ILogger _logger;

        /// <summary>
        /// Provides access to a logger for the process
        /// </summary>
        public ILogger ProcessLogger
        {
            get { return _logger ?? ( _logger = new NLogLogger( GetType().Name.ToString() ) ); }
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

        private DateTime m_LastRunTime = DateTime.Now;

        /// <summary>
        /// This time is used to determine how often (the frequency) DoProcessWork is executed.
        /// <para>It defaults to the current time on initialization.</para>
        /// </summary>
        protected DateTime LastRunTime
        {
            get
            {
                return m_LastRunTime;
            }
            set
            {
                m_LastRunTime = value;
            }
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
        /// Default Process Handling
        /// <para>Override this if you need a less frequent process run frequency.</para>
        /// <para>If you override this method don't call the base version and be sure to set the LastRunTime after your process executes</para>
        /// </summary>
        public virtual void ExecuteProcess()
        {
            // Set LastRunTime to now - frequency so the process executes immediately on startup
            LastRunTime = DateTime.Now.AddMinutes( -Frequency );

            if( DateTime.Now >= LastRunTime.AddMinutes( Frequency ) )
            {
                ProcessLogger.Debug( "Calling DoProcessWork for process [{0}].", ProcessName );

                DoProcessWork();

                LastRunTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Process Logic
        /// </summary>
        public abstract void DoProcessWork();

        /// <summary>
        /// Instantiates Thread to call ExecuteProcess on start; also instantiates the exit event for the thread
        /// <para>Any necessary setup prior to the start of thread should be done in an override that calls 
        /// base.Start() at the end</para>
        /// </summary>
        public virtual Task StartAsync( CancellationToken cancellationToken )
        {
            try
            {
                // If we can't start the process just call the task complete and don't start/configure the timer
                if (!CanStartProcess)
                {
                    return Task.CompletedTask;
                }

                ProcessLogger.Info( string.Format( "*** Start process [{0}] ***", ProcessName ) );

                _timer = new Timer(
                    ( e ) => ExecuteProcess(),
                    null,
                    TimeSpan.Zero,
                    TimeSpan.FromMinutes( Frequency ) );

                ProcessLogger.Info( ProcessName + " has been successfully started." );
            }
            catch( Exception err )
            {
                ProcessLogger.ErrorException( err, string.Format( "Service [{0}] threw an exception during startup", ProcessName ) );
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Base version of stop provides basic implementation to stop any instantiated threads gracefully
        /// and log any errors.
        /// </summary>
        public virtual Task StopAsync( CancellationToken cancellationToken )
        {
            try
            {
                ProcessLogger.Info( string.Format( "*** Stop process [{0}] ***", ProcessName ) );

                // Stop thread
                _timer?.Change( Timeout.Infinite, 0 );

                ProcessLogger.Debug( ProcessName + " process has been successfully stopped." );
            }
            catch( Exception err )
            {
                ProcessLogger.ErrorException( err, "Exception caught during process (" + ProcessName + ") stop: " );
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
