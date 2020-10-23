using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Proxies.Models;

namespace Novibet.Service.IpGeolocation.Core.Configuration
{
    public class MappingProfileConfig : Profile
    {
        public MappingProfileConfig()
        {
            CreateMap<IpDetailsResponse, IPLookupDetails>().ReverseMap();
        }
    }
}
