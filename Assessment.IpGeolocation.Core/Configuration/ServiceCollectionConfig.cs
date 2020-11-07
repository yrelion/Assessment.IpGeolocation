using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Assessment.IpGeolocation.Common.Interfaces;
using Assessment.IpGeolocation.Common.Providers;
using Assessment.IpGeolocation.Core.Services;

namespace Assessment.IpGeolocation.Core.Configuration
{
    public static class ServiceCollectionConfig
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddServices();
            services.AddHostedServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICacheProvider, CacheProvider>();
        }

        private static void AddHostedServices(this IServiceCollection services)
        {
            services.AddSingleton<IPGeolocationBatchUpdateService>();
            services.AddHostedService(provider => provider.GetService<IPGeolocationBatchUpdateService>());
        }
    }
}
