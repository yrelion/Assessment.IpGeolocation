using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Novibet.Service.IpGeolocation.Common.Interfaces;
using Novibet.Service.IpGeolocation.Core.Requests;
using Novibet.Service.IpGeolocation.Proxies.Interfaces;

namespace Novibet.Service.IpGeolocation.Core.Handlers
{
    public class GetIpDetailsHandler : IRequestHandler<GetIpDetailsQuery, IPDetails>
    {
        private readonly IIPInfoProvider _ipInfoProvider;

        public GetIpDetailsHandler(IIPInfoProvider ipInfoProvider)
        {
            _ipInfoProvider = ipInfoProvider;
        }

        public async Task<IPDetails> Handle(GetIpDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = _ipInfoProvider.GetDetails(request.IP);
            return result;
        }
    }
}
