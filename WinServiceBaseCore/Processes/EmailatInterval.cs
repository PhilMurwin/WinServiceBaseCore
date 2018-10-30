using System;
using WinServiceBaseCore.App_Entry;
using WinServiceBaseCore.Framework;

namespace WinServiceBaseCore.Processes
{
    public class EmailTest : ProcessBase
    {
        public override string StopCode
        {
            get { return "ExitEmailTest"; }
        }

        public override bool CanStartProcess
        {
            get { return ConfigKeys.EmailTest; }
        }

        public override int Frequency
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Send message to logger once a minute
        /// </summary>
        public override void DoProcessWork()
        {
            //Write current time to eventlog
            var logMessage = string.Format( "The current time is: {0}.", DateTime.Now.ToString( "HH:mm:ss tt" ) );
            ProcessLogger.Info( logMessage );
        }
    }
}
