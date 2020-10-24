using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Core.Services;

namespace Novibet.Service.IpGeolocation.Core.Models
{
    public class GeolocationBatchUpdateJob : BackgroundJob
    {
        public readonly IPGeolocationProcessor Processor;

        public GeolocationBatchUpdateJob(object request, IPGeolocationProcessor processor) : base(request)
        {
            Processor = processor;
        }
    }
}
