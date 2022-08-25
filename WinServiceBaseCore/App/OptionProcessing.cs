using WinServiceBaseCore.Processes;

namespace WinServiceBaseCore.App
{
    public static class OptionProcessing
    {
        public static void WriteToFile()
        {
            var svc = new WriteToFile();
            svc.DoProcessWork();
        }
    }
}
