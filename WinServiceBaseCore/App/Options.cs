using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.App
{
    [Verb("automation", isDefault: true, HelpText = "Call Automation Tasks")]
    public class Options
    {
        [Option('w', "WriteFile", Default = false, HelpText = "Write a test file to a folder")]
        public bool WriteToFile { get; set; }

        /**********************************************************************
        * Option Action Methods
        **********************************************************************
        *
        * Make sure you await async methods or they won't get a chance to run!
        * 
        *********************************************************************/
        #region Option Action Methods
        public static void DoProcessWorkAsync<T>(IHost host) where T : IProcessBase
        {
            var svc = host.Services.GetRequiredService<T>();
            svc.DoProcessWorkAsync(CancellationToken.None).GetAwaiter().GetResult();
        }
        #endregion Option Action Methods
    }
}
