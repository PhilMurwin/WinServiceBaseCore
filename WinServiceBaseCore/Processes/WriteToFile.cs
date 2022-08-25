using System;
using System.IO;
using WinServiceBaseCore.App;
using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.Processes
{
    public class WriteToFile : ProcessBase
    {
        public override string StopCode => "ExitWriteFile";

        public override bool CanStartProcess => ConfigKeys.WriteToFile;

        public override int Frequency => ConfigKeys.WriteToFileFrequency;

        public override void DoProcessWork()
        {
            try
            {
                var filename = $"TestFile_{DateTime.Now:yyyyMMdd-HHmmss}.txt";
                var testFileText = $"This is a test file to confirm that we have file access to wherever this file has been written.";

                var filePath = Path.Combine(ConfigKeys.WriteToFileDir, filename);
                File.WriteAllText(filePath, testFileText);
            }
            catch (Exception err)
            {
                ProcessLogger.Error(err, "An error occurred when trying to confirm file access.");
            }
        }
    }
}
