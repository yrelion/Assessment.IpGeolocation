using Assessment.IpGeolocation.Common.Interfaces;

namespace Assessment.IpGeolocation.Proxies.Interfaces
{
    public interface IIPInfoProvider
    {
        IPDetails GetDetails(string ip);
    }
}
