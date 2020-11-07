using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.IpGeolocation.Common.Interfaces
{
    public interface IBackgroundJob
    {
        Guid Id { get; }
        object Request { get; }
    }
}
