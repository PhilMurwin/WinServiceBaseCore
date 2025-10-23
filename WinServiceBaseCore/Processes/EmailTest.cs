using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WinServiceBaseCore.App;
using WinServiceBaseCore.Infrastructure;
using WinServiceBaseCore.Models.AppSettings;
using WinServiceBaseCore.Models.ProcessSettings;

namespace WinServiceBaseCore.Processes
{
    public class EmailTest : ProcessBase<EmailTestSettings>
    {
        SMTPSettings _smtpSettings;

        public EmailTest(IOptionsMonitor<EmailTestSettings> settings, ILoggerFactory loggerFactory, IOptions<SMTPSettings> smtpSettings) : base(settings, loggerFactory)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public override Task DoProcessWorkAsync(CancellationToken token)
        {
            var logMessage = $"{ProcessName}: The current time is: {DateTime.Now:HH:mm:ss tt}.";
            ProcessLogger.LogInformation(logMessage);

            //SendTestEmail(logMessage);

            return Task.CompletedTask;
        }

        public void SendTestEmail(string body)
        {
            Utils.SendEmail(_smtpSettings, _settings.CurrentValue.MailTo, "Email Test", body);
        }
    }
}
