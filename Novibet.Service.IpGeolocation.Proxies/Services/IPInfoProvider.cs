using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Novibet.Service.IpGeolocation.Common.Abstractions;
using Novibet.Service.IpGeolocation.Proxies.Configuration;
using Novibet.Service.IpGeolocation.Proxies.Interfaces;
using Novibet.Service.IpGeolocation.Proxies.Models;
using Novibet.Service.IpGeolocation.Proxies.Services;

namespace Novibet.Service.IpGeolocation.Proxies
{
    public class IPInfoProvider : ServiceProxyBase, IIPInfoProvider
    {
        public IPInfoProvider(IServiceProxyFactory<IpStackServiceProxySettings> factory)
        {
            Proxy = factory.Create();
        }

        public IPDetails GetDetails(string ip)
        {
            var result = Request<IpDetailsResponse>($"{ip}", HttpMethod.Get);
            return result;
        }
    }
}
