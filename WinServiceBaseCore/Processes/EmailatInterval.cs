using System;
using WinServiceBaseCore.App_Entry;
using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.Processes
{
    public class EmailTest : ProcessBase
    {
        public override string StopCode => "ExitEmailTest";

        public override bool CanStartProcess => ConfigKeys.EmailTest;

        public override int Frequency => ConfigKeys.EmailTestFrequency;

        public override void DoProcessWork()
        {
            var logMessage = $"The current time is: {DateTime.Now:HH:mm:ss tt}.";
            ProcessLogger.Info( logMessage );
        }
    }
}
