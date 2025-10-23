using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.Models.ProcessSettings
{
    public class EmailTestSettings : BaseProcessSettings
    {
        public static string SectionName => "EmailTest";
        public override string ConfigurationSectionName => SectionName;

        public string MailTo { get; set; }
    }
}
