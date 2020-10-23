﻿using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Common.Models;

namespace Novibet.Service.IpGeolocation.Proxies.Models
{
    /// <summary>
    /// Represents an unavailable service error
    /// </summary>
    public class IPServiceNotAvailableException : ApiException
    {
        public IPServiceNotAvailableException(string message) : base(message) { }
    }
}
