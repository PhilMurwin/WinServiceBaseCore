using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace WinServiceBaseCore.Infrastructure
{
    public static class HostedServicesExtension
    {
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            var workers = typeof(ProcessBase).GetTypeInfo().Assembly.DefinedTypes
                .Where( t => t.IsSubclassOf(typeof(ProcessBase)))
                .Where( p => ((IProcessBase) Activator.CreateInstance(p.AsType())).CanStartProcess )
                .Select( p => p.AsType() );

            foreach ( var worker in workers )
            {
                if ( typeof( IHostedService ).IsAssignableFrom( worker ) )
                {
                    services.AddTransient(typeof(IHostedService), worker);
                }
            }

            return services;
        }
    }
}
