using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Assessment.IpGeolocation.Common.Models;

namespace Assessment.IpGeolocation.Core.Requests.Commands
{
    public class UpdateBatchIPGeolocationCommand : IRequest<Guid>
    {
        public readonly IEnumerable<IPGeolocationUpdateRequest> Request;

        public UpdateBatchIPGeolocationCommand(IEnumerable<IPGeolocationUpdateRequest> request)
        {
            Request = request;
        }
    }
}
