using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WinServiceBaseCore.Infrastructure;
using WinServiceBaseCore.Models.ProcessSettings;

namespace WinServiceBaseCore.Processes
{
    /// <summary>
    /// Creates a log entry
    /// Useful as a test of the service framework and basic layout of a process
    /// </summary>
    public class BasicTimeLogger : ProcessBase<BasicTimeLoggerSettings>
    {
        public BasicTimeLogger(IOptionsMonitor<BasicTimeLoggerSettings> settings, ILoggerFactory loggerFactory) : base(settings, loggerFactory) { }

        public override Task DoProcessWorkAsync(CancellationToken token)
        {
            //Write current time to log
            var logMessage = $"The current time is: {DateTime.Now:HH:mm:ss tt}.";
            ProcessLogger.LogInformation(logMessage);

            return Task.CompletedTask;
        }
    }
}
