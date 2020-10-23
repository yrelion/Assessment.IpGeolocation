namespace Novibet.Service.IpGeolocation.Proxies.Interfaces
{
    public interface IIPInfoProvider
    {
        IPDetails GetDetails(string ip);
    }
}
