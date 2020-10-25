using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Common.Interfaces;

namespace Novibet.Service.IpGeolocation.Common.Models
{
    public class CacheSettings
    {
        public const string Name = "Cache";
        public InMemoryCacheSettings InMemory { get; set; }
    }

    public class InMemoryCacheSettings : IInMemoryCacheSettings
    {
        public const string Name = "Cache:InMemory";

        public int AbsoluteExpirationMinutes { get; set; }
    }
}
