using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Novibet.Service.IpGeolocation.Common.Models;

namespace Novibet.Service.IpGeolocation.Core.Requests.Commands
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
