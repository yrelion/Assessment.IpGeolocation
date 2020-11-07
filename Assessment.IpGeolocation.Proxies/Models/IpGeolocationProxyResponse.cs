using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Assessment.IpGeolocation.Common.Interfaces;

namespace Assessment.IpGeolocation.Proxies.Models
{
    public class IpGeolocationProxyResponse : IPDetails
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
