using Microsoft.Extensions.Hosting;

namespace WinServiceBaseCore.App_Entry
{
    public class Program
    {
        // Initial article detailing setting up .net core services
        // https://dotnetcoretutorials.com/2019/12/07/creating-windows-services-in-net-core-part-3-the-net-core-worker-way/
        public static void Main( string[] args )
        {
            CreateHostBuilder( args ).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder( string[] args )
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices( ( hostContext, services ) =>
                {
                    ServiceConfig.RegisterServices( services );
                } )
                // Configure NLog Logging: https://github.com/NLog/NLog/wiki/Getting-started-with-.NET-Core-2---Console-application
                //.ConfigureLogging( logBuilder =>
                //{
                //    logBuilder.SetMinimumLevel( LogLevel.Trace );
                //    logBuilder.AddNLog( "NLog.config" )
                //        .AddFilter( "Microsoft", LogLevel.Warning );
                //})
                .UseWindowsService();
        }
    }
}
