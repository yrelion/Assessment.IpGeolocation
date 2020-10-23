using System.Net.Http;
using Novibet.Service.IpGeolocation.Common.Interfaces;
using Novibet.Service.IpGeolocation.Proxies.Configuration;
using Novibet.Service.IpGeolocation.Proxies.Interfaces;
using Novibet.Service.IpGeolocation.Proxies.Models;

namespace Novibet.Service.IpGeolocation.Proxies.Services
{
    public class IPInfoProvider : ServiceProxyBase, IIPInfoProvider
    {
        public IPInfoProvider(IServiceProxyFactory<IpStackServiceProxySettings> factory)
        {
            Proxy = factory.Create();
        }

        public IPDetails GetDetails(string ip)
        {
            var result = RequestData<IpDetailsResponse>($"{ip}", HttpMethod.Get);
            return result;
        }
    }
}
