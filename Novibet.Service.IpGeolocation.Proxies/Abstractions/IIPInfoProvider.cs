using System;
using System.Collections.Generic;
using System.Text;

namespace Novibet.Service.IpGeolocation.Proxies.Abstractions
{
    public interface IIPInfoProvider
    {
        IPDetails GetDetails(string ip);
    }
}
