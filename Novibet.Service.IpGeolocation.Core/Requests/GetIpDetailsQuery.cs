using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Novibet.Service.IpGeolocation.Common.Models;

namespace Novibet.Service.IpGeolocation.Core.Requests
{
    public class GetIpDetailsQuery : IRequest<IPGeolocation>
    {
        public readonly string IP;

        public GetIpDetailsQuery(string ip)
        {
            IP = ip;
        }
    }
}
