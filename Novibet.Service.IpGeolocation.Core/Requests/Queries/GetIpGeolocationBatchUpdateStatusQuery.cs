using System;
using MediatR;
using Novibet.Service.IpGeolocation.Common.Models;

namespace Novibet.Service.IpGeolocation.Core.Requests.Queries
{
    public class GetIpGeolocationBatchUpdateStatusQuery : IRequest<BackgroundJobStatusType>
    {
        public readonly Guid JobId;

        public GetIpGeolocationBatchUpdateStatusQuery(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
