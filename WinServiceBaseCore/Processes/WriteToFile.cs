using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WinServiceBaseCore.Infrastructure;
using WinServiceBaseCore.Models.ProcessSettings;

namespace WinServiceBaseCore.Processes
{
    public class WriteToFile : ProcessBase<WriteToFileSettings>
    {
        public WriteToFile(IOptionsMonitor<WriteToFileSettings> settings, ILoggerFactory loggerFactory) : base(settings, loggerFactory)
        {
        }

        public override Task DoProcessWorkAsync(CancellationToken token)
        {
            try
            {
                var filename = $"TestFile_{DateTime.Now:yyyyMMdd-HHmmss}.txt";
                var testFileText = $"This is a test file to confirm that we have file access to wherever this file has been written.";

                var filePath = Path.Combine(_settings.CurrentValue.Path, filename);
                File.WriteAllText(filePath, testFileText);
            }
            catch (Exception err)
            {
                ProcessLogger.LogError(err, "An error occurred when trying to confirm file access.");
            }

            return Task.CompletedTask;
        }
    }
}
