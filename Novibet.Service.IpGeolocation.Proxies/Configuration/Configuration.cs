using Microsoft.Extensions.DependencyInjection;
using Novibet.Service.IpGeolocation.Common.Abstractions;
using Novibet.Service.IpGeolocation.Proxies.Infrastructure;
using Novibet.Service.IpGeolocation.Proxies.Interfaces;

namespace Novibet.Service.IpGeolocation.Proxies.Configuration
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
