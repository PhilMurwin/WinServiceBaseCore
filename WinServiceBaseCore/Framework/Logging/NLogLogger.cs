using NLog;
using System;
using System.Globalization;

namespace WinServiceBaseCore.Framework.Logging
{
    public class NLogLogger: ILogger
    {
        private readonly Type _wrapperType = typeof( NLogLogger );
        private readonly Logger _logger;

        /// <summary>
        /// NLog wrapper constructor, requires a logger instance name in order to differentiate the various loggers used throughout the system
        /// </summary>
        /// <param name="loggerInstanceName">Name of the logger instance to be configured
        public NLogLogger( string loggerInstanceName )
        {
            _logger = LogManager.GetLogger( loggerInstanceName );
        }

        /// <summary>
        /// NLog wrapper constructor, requires a type to differentiate the various loggers used throughout the system
        /// </summary>
        /// <param name="loggerType">Type of the logger instance to be configured, Type.FullName will be used as the logger name</param>
        public NLogLogger( Type loggerType ) : this( loggerType.FullName ) { }


        /// <summary>
        /// Most detailed information. Expect these to be written to logs only. Since version 1.2.12
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Trace( string message, params object[] args )
        {
            LogStandard( LogLevel.Trace, message, args );
        }

        /// <summary>
        /// Detailed information on the flow through the system.  Expect these to be written to logs only.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Debug( string message, params object[] args )
        {
            LogStandard( LogLevel.Debug, message, args );
        }

        /// <summary>
        /// Interesting runtime events(startup/shutdown). Expect these to be immediately visible on a console, so be conservative and keep to a minimum.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Info( string message, params object[] args )
        {
            LogStandard( LogLevel.Info, message, args );
        }

        /// <summary>
        /// Use of deprecated APIs, poor use of API, 'almost' errors, other runtime situations that are undesirable or unexpected, but not necessarily "wrong".
        /// <para>Expect these to be immediately visible on a status console.</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Warn( string message, params object[] args )
        {
            LogStandard( LogLevel.Warn, message, args );
        }

        /// <summary>
        /// Runtime errors or unexpected conditions. Expect these to be immediately visible on a status console.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Error( string message, params object[] args )
        {
            LogStandard( LogLevel.Error, message, args );
        }

        /// <summary>
        /// Severe errors that cause premature termination. Expect these to be immediately visible on a status console.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Fatal( string message, params object[] args )
        {
            LogStandard( LogLevel.Fatal, message, args );
        }

        /// <summary>
        /// Runtime errors or unexpected conditions. Expect these to be immediately visible on a status console and emailed to the dev team.
        /// </summary>
        /// <param name="err"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void ErrorException( Exception err, string message, params object[] args )
        {
            LogException( LogLevel.Error, err, message, args );
        }

        /// <summary>
        /// Severe errors that cause premature termination. Expect these to be immediately visible on a status console and emailed to the dev team.
        /// </summary>
        /// <param name="err"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void FatalException( Exception err, string message, params object[] args )
        {
            LogException( LogLevel.Fatal, err, message, args );
        }

        /// <summary>
        /// Creates a log entry
        /// </summary>
        private void LogStandard( LogLevel logLevel, string message, params object[] args )
        {
            var theEvent = LogEventInfo.Create( logLevel, _logger.Name, CultureInfo.InvariantCulture, message, args );
            _logger.Log( _wrapperType, theEvent );
        }

        /// <summary>
        /// Creates a log entry that includes exception details
        /// </summary>
        private void LogException( LogLevel logLevel, Exception err, string message, params object[] args )
        {
            var theEvent = LogEventInfo.Create( logLevel, _logger.Name, CultureInfo.InvariantCulture, message, args );
            theEvent.Exception = err;

            _logger.Log( _wrapperType, theEvent );
        }
    }
}
