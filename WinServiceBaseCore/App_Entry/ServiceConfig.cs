using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using WinServiceBaseCore.Infrastructure;

namespace WinServiceBaseCore.App_Entry
{
    public class ServiceConfig
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Might have useful code for dynamically registering hosted services
            // https://forums.asp.net/t/2164416.aspx?services+AddHostedService

            services.AddHostedServices( new List<Assembly> { Assembly.GetExecutingAssembly() } );

        }
    }
}
