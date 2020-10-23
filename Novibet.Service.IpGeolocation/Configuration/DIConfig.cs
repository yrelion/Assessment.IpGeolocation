using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Novibet.Service.IpGeolocation.Core.Configuration;
using Novibet.Service.IpGeolocation.Core.Handlers;
using Novibet.Service.IpGeolocation.Proxies.Configuration;

namespace Novibet.Service.IpGeolocation.Configuration
{
    public static class DIConfig
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Proxy Settings
            services.Configure<ProxySettings>(configuration.GetSection(ProxySettings.Name));
            services.Configure<IpStackServiceProxySettings>(configuration.GetSection(IpStackServiceProxySettings.Name));
            services.AddSingleton(x => x.GetService<IOptions<ProxySettings>>()?.Value);
            services.AddSingleton(x => x.GetService<IOptions<IpStackServiceProxySettings>>()?.Value);

            // Service Proxies
            services.AddRestProxies();

            // Handlers
            services.AddMediatR(typeof(BaseRequestHandler).GetTypeInfo().Assembly);

            // Mapping Profiles
            services.AddAutoMapper(typeof(MappingProfileConfig).GetTypeInfo().Assembly);
        }
    }
}
