using System;
using System.Collections.Generic;
using System.Text;

namespace Novibet.Service.IpGeolocation.Proxies.Abstractions
{
    public interface IPDetails
    {
        string City { get; set; }
        string Country { get; set; }
        string Continent { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}
