using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace WinServiceBaseCore.Infrastructure
{
    public static class SettingsRegistrationExtension
    {
        public static void RegisterAllAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var settingsTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(IAppSettings).IsAssignableFrom(t))
                .ToList();

            foreach (var type in settingsTypes)
            {
                var sectionNameProp = type.GetProperty("SectionName", BindingFlags.Public | BindingFlags.Static);
                if (sectionNameProp == null)
                {
                    throw new InvalidOperationException($"{type.Name} must define a public static SectionName property.");
                }

                var sectionName = sectionNameProp.GetValue(null) as string;
                var configSection = configuration.GetSection(sectionName);

                var method = typeof(OptionsConfigurationServiceCollectionExtensions)
                    .GetMethods()
                    .First(m => m.Name == "Configure" && m.GetGenericArguments().Length == 1);

                var genericMethod = method.MakeGenericMethod(type);
                genericMethod.Invoke(null, new object[] { services, configSection });
            }
        }
    }
}
