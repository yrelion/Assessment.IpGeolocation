using System.Net.Http;
using Assessment.IpGeolocation.Common.Interfaces;
using Assessment.IpGeolocation.Common.Models;
using Assessment.IpGeolocation.Proxies.Interfaces;
using Assessment.IpGeolocation.Proxies.Models;

namespace Assessment.IpGeolocation.Proxies.Services
{
    public class IPInfoProvider : ServiceProxyBase, IIPInfoProvider
    {
        public IPInfoProvider(IServiceProxyFactory<IpStackServiceProxySettings> factory)
        {
            Proxy = factory.Create();
        }

        public IPDetails GetDetails(string ip)
        {
            var result = RequestData<IpGeolocationProxyResponse>($"{ip}", HttpMethod.Get);
            return result;
        }
    }
}
