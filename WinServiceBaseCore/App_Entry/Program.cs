using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using WinServiceBaseCore.Framework.ServiceBase;

namespace WinServiceBaseCore.App_Entry
{
    class Program
    {
        static async System.Threading.Tasks.Task Main( string[] args )
        {
            var isService = !( Debugger.IsAttached || args.Contains( "--console" ) );

            var host = new HostBuilder()
                .ConfigureServices( ( hostContext, services ) =>
                {
                    ServiceConfig.RegisterServices( services );
                } )
                .UseConsoleLifetime();

            if( isService )
            {
                await host.RunAsServiceAsync();
            }
            else
            {
                await host.RunConsoleAsync();
            }

        }
    }
}
