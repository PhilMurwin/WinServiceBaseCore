using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace WinServiceBaseCore.Infrastructure
{
    public static class ServiceRegistrationExtension
    {
        public static void RegisterBackgroundProcesses(this IServiceCollection services, IConfiguration configuration)
        {
            // Get all types inheriting from ProcessBase
            var processTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == typeof(ProcessBase<>)
                    )
                .ToList();

            foreach (var processType in processTypes)
            {
                try
                {
                    // Use ConfigurationSectionName property to dynamically get the configurations for the process
                    var configType = processType.BaseType.GetGenericArguments()[0]; // Get TConfig

                    // Look for a static SectionName property on the settings type
                    var sectionNameProp = configType.GetProperty("SectionName", BindingFlags.Public | BindingFlags.Static);

                    if (sectionNameProp == null)
                    {
                        throw new InvalidOperationException($"{configType.Name} must define a public static SectionName property.");
                    }

                    var configurationKey = sectionNameProp.GetValue(null) as string;

                    if (!string.IsNullOrEmpty(configurationKey))
                    {
                        // Bind the configuration for this process
                        var configSection = configuration.GetSection(configurationKey);

                        // Dynamically call services.Configure<TConfig>(configSection)
                        var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
                            .GetMethods()
                            .First(m => m.Name == "Configure" && m.GetGenericArguments().Length == 1);

                        var genericConfigureMethod = configureMethod.MakeGenericMethod(configType);
                        genericConfigureMethod.Invoke(null, new object[] { services, configSection });

                        // Register concrete type so it can be resolved directly
                        services.AddSingleton(processType);

                        // Also register it as IHostedService (required for .NET to run background services)
                        services.AddSingleton(typeof(IHostedService), processType);
                    }
                }
                // Catch an error here so we can throw a more informative error
                catch (Exception err)
                {
                    var msg = $"RegisterBackgroundProcesses - Error registering process {processType.Name} -> {err.Message}";
                    throw new Exception(msg, err);
                }
            }
        }
    }
}
