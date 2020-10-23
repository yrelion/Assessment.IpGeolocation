using Novibet.Service.IpGeolocation.Common.Interfaces;

namespace Novibet.Service.IpGeolocation.Proxies.Interfaces
{
    public interface IIPInfoProvider
    {
        IPDetails GetDetails(string ip);
    }
}
