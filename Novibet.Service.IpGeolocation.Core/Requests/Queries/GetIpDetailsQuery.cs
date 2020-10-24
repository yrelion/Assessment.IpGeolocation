using MediatR;
using Novibet.Service.IpGeolocation.Common.Models;

namespace Novibet.Service.IpGeolocation.Core.Requests.Queries
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
