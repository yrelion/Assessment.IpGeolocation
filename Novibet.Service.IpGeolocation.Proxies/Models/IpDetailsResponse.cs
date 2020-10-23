using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Novibet.Service.IpGeolocation.Common.Interfaces;

namespace Novibet.Service.IpGeolocation.Proxies.Models
{
    public class IpDetailsResponse : IPDetails
    {
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("country_name")]
        public string Country { get; set; }
        [JsonProperty("continent_name")]
        public string Continent { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
