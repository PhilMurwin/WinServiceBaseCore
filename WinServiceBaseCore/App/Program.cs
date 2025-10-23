using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.App
{
    internal class Program
    {
        private static ILogger<Program> _logger;
        public static void Main( string[] args )
        {
            try
            {
                var callerIsMain = true;

                SetLogger();

                // Parse command line options
                var cliArgs = CommandLine.Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(opts => CommandLineHandlers.RunWithDefaultOptions(opts, callerIsMain))
                    .WithNotParsed(errors => CommandLineHandlers.HandleParseError(errors));
            }
            catch (Exception err)
            {
                // NLog: catch any exception and log it.
                _logger.LogDebug($"Could not startup due to an exception: {err.Message}");
            }
        }

        private static void SetLogger()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
                builder.AddNLog(); // Use NLog as the logging provider
            });
            _logger = loggerFactory.CreateLogger<Program>();
        }

        public static void StartWinService()
        {
            BuildHost().Run();
        }

        public static IHost BuildHost(string[] args = null)
        {
            return CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder( string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration( (context, config) => 
                {
                    var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

                    config.SetBasePath(Utils.AssemblyDirectory)
                        .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appSettings.{environment}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices( ( context, services ) =>
                {
                    // Register NLog as the logging provider
                    services.AddLogging(builder =>
                    {
                        builder.ClearProviders(); // Optionally clear default providers
                        builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        builder.AddNLog("NLog.config"); // Add NLog as the logging provider
                    });

                    // Dynamically register app settings
                    services.RegisterAllAppSettings(context.Configuration);

                    // Dynamically discover and register all ProcessBase implementations
                    services.RegisterBackgroundProcesses(context.Configuration);
                })
                .UseWindowsService();
        }
    }
}
