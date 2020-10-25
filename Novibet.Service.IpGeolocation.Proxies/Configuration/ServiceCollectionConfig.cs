using Microsoft.Extensions.DependencyInjection;
using Novibet.Service.IpGeolocation.Common.Interfaces;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Proxies.Factories;
using Novibet.Service.IpGeolocation.Proxies.Interfaces;
using Novibet.Service.IpGeolocation.Proxies.Services;

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
