using Microsoft.Extensions.Logging;
using System;
using WinServiceBaseCore.App_Entry;
using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.Processes
{
    public class EmailTest : ProcessBase<EmailTest>
    {
        public override string StopCode => "ExitEmailTest";

        public override bool CanStartProcess => ConfigKeys.EmailTest;

        public override int Frequency => 1;

        public EmailTest( ILogger<EmailTest> logger ) : base( logger )
        {

        }

        public override void DoProcessWork()
        {
            var logMessage = $"The current time is: {DateTime.Now:HH:mm:ss tt}.";
            ProcessLogger.LogInformation( logMessage );
        }
    }
}
