using Microsoft.Extensions.DependencyInjection;
using WinServiceBaseCore.Processes;

namespace WinServiceBaseCore.App_Entry
{
    public class ServiceConfig
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Register Services
            services.AddHostedService<BasicTimeLogger>();
            services.AddHostedService<EmailTest>();
        }
    }
}
