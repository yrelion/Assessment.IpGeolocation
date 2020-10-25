using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Common.Interfaces;

namespace Novibet.Service.IpGeolocation.Common.Models
{
    public class ProxySettings
    {
        public const string Name = "Proxy";
        public IpStackServiceProxySettings IpStack { get; set; }
    }

    public abstract class ServiceProxySettings : IServiceProxySettings
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string BaseUrl { get; set; }
        public string ContentType { get; set; }
        public string AccessKey { get; set; }
    }

    public class IpStackServiceProxySettings : ServiceProxySettings
    {
        public const string Name = "Proxy:IPStack";
    }
}
