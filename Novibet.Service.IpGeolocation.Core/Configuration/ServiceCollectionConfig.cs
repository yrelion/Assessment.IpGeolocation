using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Novibet.Service.IpGeolocation.Common.Interfaces;
using Novibet.Service.IpGeolocation.Core.Services;

namespace Novibet.Service.IpGeolocation.Core.Configuration
{
    public static class ServiceCollectionConfig
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICacheProvider, CacheProvider>();
        }
    }
}
