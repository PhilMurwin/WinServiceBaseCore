using System;
using WinServiceBaseCore.App;
using WinServiceBaseCore.Infrastructure;
using WinServiceBaseCore.Models;

namespace WinServiceBaseCore.Processes
{
    public class EmailTest : ProcessBase
    {
        public override string StopCode => "ExitEmailTest";

        public override bool CanStartProcess => ConfigKeys.EmailTest;

        public override int Frequency => ConfigKeys.EmailTestFrequency;

        private NLog.Logger logger;

        public EmailTest() : base()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        public override void DoProcessWork()
        {
            var logMessage = $"{ProcessName}: The current time is: {DateTime.Now:HH:mm:ss tt}.";
            logger.Info(logMessage);
            
            //SendTestEmail(logMessage);
        }

        public void SendTestEmail(string body)
        {
            var smtpSettings = new SMTPSettings
            {
                Host = ConfigKeys.SMTPHost,
                Port = ConfigKeys.SMTPPort,
                Username = ConfigKeys.SMTPUser,
                Password = ConfigKeys.SMTPPass,
                SenderEmail = ConfigKeys.SMTPUser,
                SenderName = "Service Test"
            };

            Utils.SendEmail(smtpSettings, ConfigKeys.EmailTestMailTo, "Email Test", body);
        }
    }
}
