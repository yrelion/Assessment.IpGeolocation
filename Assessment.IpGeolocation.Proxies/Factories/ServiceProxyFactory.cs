﻿using Microsoft.Extensions.Options;
using Assessment.IpGeolocation.Common.Factories;
using Assessment.IpGeolocation.Common.Interfaces;
using Assessment.IpGeolocation.Common.Models;

namespace Assessment.IpGeolocation.Proxies.Factories
{
    /// <summary>
    /// Service proxy factory
    /// </summary>
    /// <typeparam name="TSettings">The <see cref="IServiceProxySettings"/> to build the service proxy with</typeparam>
    public class ServiceProxyFactory<TSettings> : ServiceProxyFactoryBase<TSettings>
        where TSettings : class, IServiceProxySettings, new()
    {
        public ServiceProxyFactory(IOptions<TSettings> options)
        {
            Settings = options.Value;
        }

        /// <summary>
        /// Creates an <see cref="IServiceProxy"/> using its corresponding <see cref="IServiceProxySettings"/>
        /// </summary>
        public override IServiceProxy Create()
        {
            var proxy = new ServiceProxy(config => {
                config.ServiceId = Settings.ServiceId;
                config.ServiceName = Settings.ServiceName;
                config.BaseUrl = Settings.BaseUrl;
                config.ContentType = Settings.ContentType;
                config.AccessKey = Settings.AccessKey;
            });

            return proxy;
        }
    }
}
