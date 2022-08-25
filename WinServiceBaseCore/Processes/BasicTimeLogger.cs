using System;
using WinServiceBaseCore.App;
using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.Processes
{
    /// <summary>
    /// Creates a log entry
    /// Useful as a test of the service framework and basic layout of a process
    /// </summary>
    public class BasicTimeLogger : ProcessBase
    {
        public override string StopCode => "ExitLogger";

        public override bool CanStartProcess => ConfigKeys.BasicTimeLogger;

        public override int Frequency => ConfigKeys.BasicTimeLoggerFrequency;

        public override void DoProcessWork()
        {
            //Write current time to log
            var logMessage = $"The current time is: {DateTime.Now:HH:mm:ss tt}.";
            ProcessLogger.Info( logMessage );
        }
    }
}
