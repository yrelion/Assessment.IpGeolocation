using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Novibet.Service.IpGeolocation.Common.Interfaces;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Proxies.Models;

namespace Novibet.Service.IpGeolocation.Core.Requests
{
    public class GetIpDetailsQuery : IRequest<IPLookupDetails>
    {
        public readonly string IP;

        public GetIpDetailsQuery(string ip)
        {
            IP = ip;
        }
    }
}
