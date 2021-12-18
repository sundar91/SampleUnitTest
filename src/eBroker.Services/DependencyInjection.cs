using Microsoft.Extensions.DependencyInjection;
using eBroker.Data;
using Microsoft.Extensions.Configuration;
using eBroker.Services.Interfaces;

namespace eBroker.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataServices(configuration);

            services.AddTransient<ITraderService, TraderService>();
            services.AddTransient<IUtilityWrapper, UtilityWrapper>();
            
            return services;
        }
    }
}
