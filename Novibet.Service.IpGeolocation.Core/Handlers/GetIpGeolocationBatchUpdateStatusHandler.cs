using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Core.Requests;
using Novibet.Service.IpGeolocation.Core.Requests.Queries;
using Novibet.Service.IpGeolocation.Core.Services;

namespace Novibet.Service.IpGeolocation.Core.Handlers
{
    public class GetIpGeolocationBatchUpdateStatusHandler : IRequestHandler<GetIpGeolocationBatchUpdateStatusQuery, BackgroundJobStatusType>
    {
        private readonly IPGeolocationBatchUpdateService _batchUpdateService;

        public GetIpGeolocationBatchUpdateStatusHandler(IPGeolocationBatchUpdateService batchUpdateService)
        {
            _batchUpdateService = batchUpdateService;
        }

        public async Task<BackgroundJobStatusType> Handle(GetIpGeolocationBatchUpdateStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _batchUpdateService.GetJobStatus(request.JobId);
            return result;
        }
    }
}
