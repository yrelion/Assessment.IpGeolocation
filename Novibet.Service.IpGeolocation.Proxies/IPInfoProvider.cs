using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Proxies.Abstractions;

namespace Novibet.Service.IpGeolocation.Proxies
{
    public class IPInfoProvider : IIPInfoProvider
    {
        public IPDetails GetDetails(string ip)
        {
            throw new NotImplementedException();
        }
    }
}
