using Microsoft.Extensions.DependencyInjection;
using WinServiceBaseCore.Processes;

namespace WinServiceBaseCore.App_Entry
{
    public class ServiceConfig
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Might have useful code for dynamically registering hosted services
            // https://forums.asp.net/t/2164416.aspx?services+AddHostedService
            // This link may be relavant to dynamically registering hosted services
            //https://medium.com/swlh/creating-a-worker-service-in-asp-net-core-3-0-6af5dc780c80

            // Register Services
            if ( ConfigKeys.BasicTimeLogger )
            {
                services.AddHostedService<BasicTimeLogger>();
            }

            if ( ConfigKeys.EmailTest )
            {
                services.AddHostedService<EmailTest>();
            }
        }
    }
}
