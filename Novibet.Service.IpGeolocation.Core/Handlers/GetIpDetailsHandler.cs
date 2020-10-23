using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Core.Requests;
using Novibet.Service.IpGeolocation.Proxies.Interfaces;

namespace Novibet.Service.IpGeolocation.Core.Handlers
{
    public class GetIpDetailsHandler : IRequestHandler<GetIpDetailsQuery, IPLookupDetails>
    {
        private readonly IMapper _mapper;
        private readonly IIPInfoProvider _ipInfoProvider;

        public GetIpDetailsHandler(IMapper mapper, IIPInfoProvider ipInfoProvider)
        {
            _mapper = mapper;
            _ipInfoProvider = ipInfoProvider;
        }

        public async Task<IPLookupDetails> Handle(GetIpDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = _ipInfoProvider.GetDetails(request.IP);
            var mappedResult = _mapper.Map<IPLookupDetails>(result);

            return mappedResult;
        }
    }
}
