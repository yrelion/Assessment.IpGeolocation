using System;
using System.Collections.Generic;
using System.Text;

namespace Novibet.Service.IpGeolocation.Common.Interfaces
{
    public interface IInMemoryCacheSettings
    {
        int AbsoluteExpirationMinutes { get; set; }
    }
}
