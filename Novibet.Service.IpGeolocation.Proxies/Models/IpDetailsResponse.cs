using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Proxies.Abstractions;

namespace Novibet.Service.IpGeolocation.Proxies.Models
{
    public class IpDetailsResponse : IPDetails
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Continent { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
