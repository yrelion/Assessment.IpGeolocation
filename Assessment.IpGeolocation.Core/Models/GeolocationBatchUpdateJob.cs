using System;
using System.Collections.Generic;
using System.Text;
using Assessment.IpGeolocation.Common.Models;
using Assessment.IpGeolocation.Core.Services;

namespace Assessment.IpGeolocation.Core.Models
{
    public class GeolocationBatchUpdateJob : BackgroundJob
    {
        public readonly IPGeolocationProcessor Processor;
        public readonly int RequestItemCount;
        public int RemainingItemCount { get; set; }

        public GeolocationBatchUpdateJob(List<IPGeolocationUpdateRequest> request, IPGeolocationProcessor processor) : base(request)
        {
            RequestItemCount = request.Count;
            RemainingItemCount = RequestItemCount;
            Processor = processor;
        }
    }
}
