namespace Novibet.Service.IpGeolocation.Common.Interfaces
{
    public interface IServiceProxySettings
    {
        string ServiceId { get; set; }
        string ServiceName { get; set; }
        string BaseUrl { get; set; }
        string ContentType { get; set; }
        string AccessKey { get; set; }
    }
}
