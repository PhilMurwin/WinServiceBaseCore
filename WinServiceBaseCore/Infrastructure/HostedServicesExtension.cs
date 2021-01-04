using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WinServiceBaseCore.Infrastructure
{
    public static class HostedServicesExtension
    {
        // https://forums.asp.net/t/2164416.aspx?services+AddHostedService
        public static IServiceCollection AddHostedServices(this IServiceCollection services, List<Assembly> workerAssemblies)
        {
            MethodInfo methodInfo = typeof( ServiceCollectionHostedServiceExtensions )
                .GetMethods()
                .FirstOrDefault( p => p.Name == nameof( ServiceCollectionHostedServiceExtensions.AddHostedService ) );

            if (methodInfo == null)
            {
                throw new Exception( $"Impossible to find the extension method '{nameof( ServiceCollectionHostedServiceExtensions.AddHostedService )}' of '{nameof( IServiceCollection )}'." );
            }

            IEnumerable<Type> hostedServices_FromAssemblies = workerAssemblies
                .SelectMany( a => a.DefinedTypes )
                .Where( t => t.IsSubclassOf(typeof(ProcessBase)))
                .Where( p => ((IProcessBase) Activator.CreateInstance(p.AsType())).CanStartProcess )
                .Select( p => p.AsType() );

            foreach ( Type hostedService in hostedServices_FromAssemblies)
            {
                if ( typeof( IHostedService ).IsAssignableFrom( hostedService ) )
                {
                    var genericMethod_AddHostedService = methodInfo.MakeGenericMethod( hostedService );
                    // this is like calling services.AddHostedServices<T>(), but with dynamic T (= backgroundService).
                    _ = genericMethod_AddHostedService.Invoke( obj: null, parameters: new object[] { services } );
                }
            }

            return services;
        }
    }
}
