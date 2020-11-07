using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Assessment.IpGeolocation.Data.Models;
using Assessment.IpGeolocation.Proxies.Models;
using IPGeolocation = Assessment.IpGeolocation.Common.Models.IPGeolocation;

namespace Assessment.IpGeolocation.Core.Configuration
{
    public class MappingProfileConfig : Profile
    {
        public MappingProfileConfig()
        {
            CreateMap<IPGeolocation, IpGeolocationProxyResponse>().ReverseMap();
            CreateMap<IPGeolocationDto, IPGeolocation>().ReverseMap();
        }
    }
}
