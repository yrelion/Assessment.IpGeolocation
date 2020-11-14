using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Assessment.IpGeolocation.Common.Models;
using Assessment.IpGeolocation.Core.Services;

namespace Assessment.IpGeolocation.Core.Requests.Queries
{
    public class GetIpGeolocationBatchUpdateStatusQuery : IRequest<BackgroundJobStatus>
    {
        public readonly Guid JobId;

        public GetIpGeolocationBatchUpdateStatusQuery(Guid jobId)
        {
            JobId = jobId;
        }

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
}
