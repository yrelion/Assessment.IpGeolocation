using System;
using System.Collections.Generic;
using System.Text;
using Assessment.IpGeolocation.Common.Models;

namespace Assessment.IpGeolocation.Proxies.Models
{
    /// <summary>
    /// Represents an unavailable service error
    /// </summary>
    public class IPServiceNotAvailableException : ApiException
    {
        public IPServiceNotAvailableException(string message) : base(message) { }
    }
}
