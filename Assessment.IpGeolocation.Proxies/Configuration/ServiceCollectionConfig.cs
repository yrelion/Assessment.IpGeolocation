using Microsoft.Extensions.DependencyInjection;
using Assessment.IpGeolocation.Common.Interfaces;
using Assessment.IpGeolocation.Common.Models;
using Assessment.IpGeolocation.Proxies.Factories;
using Assessment.IpGeolocation.Proxies.Interfaces;
using Assessment.IpGeolocation.Proxies.Services;

namespace Assessment.IpGeolocation.Proxies.Configuration
{
    public static class ServiceCollectionConfig
    {
        public static void AddRestProxies(this IServiceCollection services)
        {
            services.AddProxyFactories();
            services.AddProxies();
        }

        private static void AddProxyFactories(this IServiceCollection services)
        {
            services.AddSingleton<IServiceProxyFactory<IpStackServiceProxySettings>, ServiceProxyFactory<IpStackServiceProxySettings>>();
        }

        private static void AddProxies(this IServiceCollection services)
        {
            services.AddScoped<IIPInfoProvider, IPInfoProvider>();
        }
    }
}
