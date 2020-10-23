﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Novibet.Service.IpGeolocation.Common.Models
{
    public class IPLookupDetails
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Continent { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
