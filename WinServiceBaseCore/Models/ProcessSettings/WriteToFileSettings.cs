using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.Models.ProcessSettings
{
    public class WriteToFileSettings : BaseProcessSettings
    {
        public static string SectionName => "WriteToFile";
        public override string ConfigurationSectionName => SectionName;

        public string Path { get; set; }
    }
}
