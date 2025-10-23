using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.App
{
    public class Program
    {
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

        // Initial article detailing setting up .net core services
        // https://dotnetcoretutorials.com/2019/12/07/creating-windows-services-in-net-core-part-3-the-net-core-worker-way/
        public static void Main( string[] args )
        {
            try
            {
                // Parse command line options
                var cliArgs = CommandLine.Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(opts => RunWithOptions(opts))
                    .WithNotParsed(HandleParseError);
            }
            catch (Exception err)
            {
                // NLog: catch any exception and log it.
                Logger.Error(err, "Stopped program because of exception.");
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid seg fault on Linux)
                LogManager.Shutdown();
            }
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var err in errs)
            {
                Logger.Debug("An error occurred while parsing command line arguments: " + err.Tag);
            }
        }

        private static void RunWithOptions(Options options)
        {
            // Act on the CLI options
            // -w --WriteFile
            if (options.WriteToFile)
            {
                Logger.Info("CLI Option: WriteToFile");
                OptionProcessing.WriteToFile();
            }
            // No recognized options passed
            else
            {
                Logger.Info("*** Starting Service Base Core ***");

                StartWinService();
            }
        }

        private static void StartWinService()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder( string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration( app => 
                //{
                //    app.AddJsonFile("appsettings.json");
                //})
                .ConfigureServices( ( hostContext, services ) =>
                {
                    // Configure NLog Logging: https://github.com/NLog/NLog/wiki/Getting-started-with-.NET-Core-2---Console-application
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        loggingBuilder.AddNLog();
                    });

                    services.AddHostedServices();
                })
                .UseWindowsService(options =>
                {
                    options.ServiceName = "Win Service Base";
                });
        }
    }
}
