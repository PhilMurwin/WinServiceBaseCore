using CommandLine;

namespace WinServiceBaseCore.App
{
    public class Options
    {
        [Option('w', "WriteFile", Default = false, HelpText = "Write a test file to a folder")]
        public bool WriteToFile { get; set; }
    }
}
