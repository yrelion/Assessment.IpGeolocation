using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Data.Models;
using Novibet.Service.IpGeolocation.Proxies.Models;
using IPGeolocation = Novibet.Service.IpGeolocation.Common.Models.IPGeolocation;

namespace Novibet.Service.IpGeolocation.Core.Configuration
{
    public class MappingProfileConfig : Profile
    {
        public MappingProfileConfig()
        {
            CreateMap<IPGeolocation, IpGeolocationProxyResponse>().ReverseMap();
            CreateMap<IPGeolocationDto, IPGeolocation>().ReverseMap();
            CreateMap<IPGeolocationUpdateRequest, IPGeolocationDto>()
                .ForMember(x => x.Id, source => source.MapFrom(x => x.Ip)).ReverseMap();
        }
    }
}
