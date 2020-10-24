using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Common.Interfaces;

namespace Novibet.Service.IpGeolocation.Common.Models
{
    public abstract class BackgroundJob : IBackgroundJob
    {
        public Guid Id { get; }
        public object Request { get; }

        protected BackgroundJob(object request)
        {
            Id = Guid.NewGuid();
            Request = request;
        }
    }
}
