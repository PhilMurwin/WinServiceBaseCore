using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.Models.ProcessSettings
{
    public class BasicTimeLoggerSettings : BaseProcessSettings
    {
        public static string SectionName => "BasicTimeLogger";
        public override string ConfigurationSectionName => SectionName;
    }
}
