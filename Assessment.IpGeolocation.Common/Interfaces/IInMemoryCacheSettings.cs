using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.IpGeolocation.Common.Interfaces
{
    public interface IInMemoryCacheSettings
    {
        int AbsoluteExpirationMinutes { get; set; }
    }
}
