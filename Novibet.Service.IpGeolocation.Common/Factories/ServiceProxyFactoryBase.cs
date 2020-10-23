using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Common.Abstractions;

namespace Novibet.Service.IpGeolocation.Common.Factories
{
    /// <summary>
    /// Abstract service proxy factory
    /// </summary>
    /// <typeparam name="TSettings">The <see cref="IServiceProxySettings"/> to build the service proxy with</typeparam>
    public abstract class ServiceProxyFactoryBase<TSettings> : IServiceProxyFactory<TSettings>
        where TSettings : class, IServiceProxySettings, new()
    {
        public IServiceProxySettings Settings { get; set; }
        public abstract IServiceProxy Create();
    }
}
