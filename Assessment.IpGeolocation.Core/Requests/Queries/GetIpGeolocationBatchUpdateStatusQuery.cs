using System;
using MediatR;
using Assessment.IpGeolocation.Common.Models;

namespace Assessment.IpGeolocation.Core.Requests.Queries
{
    public class GetIpGeolocationBatchUpdateStatusQuery : IRequest<BackgroundJobStatus>
    {
        public readonly Guid JobId;

        public GetIpGeolocationBatchUpdateStatusQuery(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
