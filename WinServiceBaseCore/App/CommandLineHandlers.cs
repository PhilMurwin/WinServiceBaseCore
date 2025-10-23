using CommandLine;
using NLog;
using WinServiceBaseCore.Processes;

namespace WinServiceBaseCore.App
{
    public class CommandLineHandlers
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static int HandleParseError(IEnumerable<Error> errors)
        {
            var result = -2;

            if (errors.Any(x => x is HelpRequestedError || x is HelpVerbRequestedError || x is VersionRequestedError || x is NoVerbSelectedError))
            {
                result = -1;
            }

            if (result < -1)
            {
                foreach (var err in errors)
                {
                    _logger.Debug("An error occurred while parsing command line arguments: " + err.Tag);
                }
            }

            return result;
        }

        public static void RunWithDefaultOptions(Options options, bool callerIsMain = false)
        {
            try
            {
                using (var host = Program.BuildHost()) // Build the host
                {
                    // Act on the CLI options
                    if (options.WriteToFile)
                    {
                        _logger.Info("CLI Option: WriteToFile");
                        Options.DoProcessWorkAsync<WriteToFile>(host);
                    }
                    // If there were no options passed start the service
                    else
                    {
                        _logger.Info("*** Starting Service ***");
                        Program.StartWinService();
                    }
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, err.Message);
            }
        }
    }
}
