namespace WinServiceBaseCore.Infrastructure
{
    public interface IAppSettings
    {
        // Convention: every settings class must expose SectionName
        static abstract string SectionName { get; }
    }
}
