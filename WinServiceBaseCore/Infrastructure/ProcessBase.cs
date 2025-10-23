using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WinServiceBaseCore.Infrastructure.Extensions;

namespace WinServiceBaseCore.Infrastructure
{
    /// <summary>
    /// Base class for creating processes for the service to execute
    /// </summary>
    public abstract class ProcessBase<TConfig> : BackgroundService, IProcessBase where TConfig : BaseProcessSettings
    {
        protected const int DefaultSleepSeconds = 30 * 1000;
        protected const int MillisecondsInMinute = 60 * 1000;
        private ILogger _logger;
        private TConfig _lastKnownSettings;

        protected readonly IOptionsMonitor<TConfig> _settings;

        public ProcessBase(IOptionsMonitor<TConfig> settings, ILoggerFactory loggerFactory)
        {
            _settings = settings;
            _lastKnownSettings = _settings.CurrentValue;
            _logger = loggerFactory.CreateLogger(GetType());

            // React to configuration changes
            _settings.OnChange(OnSettingsChanged);

            // Set LastRunTime to now - frequency so the process executes immediately on startup
            LastRunTime = DateTime.Now.AddMinutes(-_settings.CurrentValue.Frequency);
        }

        /// <summary>
        /// Provides access to a logger for the process
        /// </summary>
        public ILogger ProcessLogger => _logger;

        /// <summary>
        /// Returns the name of the current process for use in error messages etc...
        /// </summary>
        public string ProcessName => GetType().Name;

        public bool Enabled => _settings.CurrentValue.Enabled;

        /// <summary>
        /// Frequncy (in minutes) to execute the process.
        /// <para>This should typically be set via a config setting that is read in the override of this property.</para>
        /// </summary>
        public int Frequency => _settings.CurrentValue.Frequency;

        /// <summary>
        /// This time is used to determine how often (the frequency) DoProcessWork is executed.
        /// <para>It defaults to the current time on initialization.</para>
        /// </summary>
        protected DateTime LastRunTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Override in subclasses to specify when this process should run (null = runs on frequency
        /// </summary>
        protected virtual TimeSpan? RunAtTime => null;

        /// <summary>
        /// Override in subclasses to specify which days the process should run (null = runs every day)
        /// </summary>
        protected virtual DayOfWeek[] RunOnDays => null;

        /// <summary>
        /// Default Process Loop
        /// <para>Sleeps the thread/process for some amount of time after each run of DoProcessWork.</para>
        /// </summary>
        protected override async Task ExecuteAsync( CancellationToken stoppingToken )
        {
            // Set LastRunTime to now - frequency so the process executes immediately on startup
            LastRunTime = DateTime.Now.AddMinutes( -Frequency );

            ProcessLogger.LogTrace($"Starting ExecuteAsync for [{ProcessName}]");
            ProcessLogger.LogDebug($"[{ProcessName}] frequency set to every {Frequency} minute(s).");

            // If cancellation signaled break out of loop
            while ( !stoppingToken.IsCancellationRequested )
            {
                var currentConfig = _settings.CurrentValue;

                ProcessLogger.LogTrace(currentConfig.ToJson());
                if (!currentConfig.Enabled)
                {
                    ProcessLogger.LogDebug($"{ProcessName} process is currently disabled. Sleeping...");
                    await Task.Delay(Frequency * MillisecondsInMinute, stoppingToken);
                    continue;
                }

                // Calculate next run time
                var now = DateTime.Now;
                // Default next run time is now + frequency
                DateTime? nextRunTime = now.AddMinutes(Frequency);

                if (RunAtTime.HasValue)
                {
                    // Compute the scheduled run time for today
                    var scheduledTime = now.Date + RunAtTime.Value;
                    bool isCorrectDay = RunOnDays == null || RunOnDays.Contains(now.DayOfWeek);

                    if (isCorrectDay && LastRunTime < scheduledTime && now >= scheduledTime)
                    {
                        ProcessLogger.LogInformation($"Running {ProcessName} at {scheduledTime} on {now.DayOfWeek}.");
                        await DoProcessWorkAsync(stoppingToken);
                        LastRunTime = now;
                    }

                    // Determine the next valid run time
                    nextRunTime = (isCorrectDay && now < scheduledTime) ? scheduledTime : scheduledTime.AddDays(1);

                    // Skip to the next valid schedule day
                    while (RunOnDays != null && !RunOnDays.Contains(nextRunTime.Value.DayOfWeek))
                    {
                        nextRunTime = nextRunTime.Value.AddDays(1);
                    }

                    ProcessLogger.LogInformation($"{ProcessName} Next run scheduled at: {nextRunTime:yyyy-MM-dd HH:mm:ss}.");
                }
                else if (now >= LastRunTime.AddMinutes(Frequency))
                {
                    ProcessLogger.LogInformation($"Running {ProcessName} at {now} on {now.DayOfWeek}.");
                    await DoProcessWorkAsync(stoppingToken);

                    LastRunTime = now;
                    nextRunTime = now.AddMinutes(Frequency);
                }

                // Sleep until next run time
                var sleepDuration = nextRunTime.Value - now;
                await Task.Delay( sleepDuration, stoppingToken );
            }

            ProcessLogger.LogInformation($"{ProcessName} is stopping.");
        }

        /// <summary>
        /// Process Logic
        /// </summary>
        public abstract Task DoProcessWorkAsync(CancellationToken token);

        private void OnSettingsChanged(TConfig newSettings)
        {
            if (_lastKnownSettings != newSettings)
            {
                ProcessLogger.LogInformation($"Configuration changed. Enabled = {newSettings.Enabled}");
                ProcessLogger.LogDebug($"[{ProcessName}] frequency set to every {Frequency} minute(s).");
                _lastKnownSettings = newSettings;
            }
        }
    }
}
