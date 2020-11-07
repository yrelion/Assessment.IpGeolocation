using MediatR;
using Assessment.IpGeolocation.Common.Models;

namespace Assessment.IpGeolocation.Core.Requests.Queries
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
