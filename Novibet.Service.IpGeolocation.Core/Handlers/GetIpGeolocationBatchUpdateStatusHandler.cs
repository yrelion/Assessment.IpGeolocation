using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Core.Requests.Queries;
using Novibet.Service.IpGeolocation.Core.Services;

namespace Novibet.Service.IpGeolocation.Core.Handlers
{
    public class GetIpGeolocationBatchUpdateStatusHandler : IRequestHandler<GetIpGeolocationBatchUpdateStatusQuery, BackgroundJobStatus>
    {
        private readonly IPGeolocationBatchUpdateService _batchUpdateService;

        public GetIpGeolocationBatchUpdateStatusHandler(IPGeolocationBatchUpdateService batchUpdateService)
        {
            _batchUpdateService = batchUpdateService;
        }

        public async Task<BackgroundJobStatus> Handle(GetIpGeolocationBatchUpdateStatusQuery request, CancellationToken cancellationToken)
        {
            var result = _batchUpdateService.GetJobStatus(request.JobId);
            return result;
        }
    }
}
