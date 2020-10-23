using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Common.Resources;

namespace Novibet.Service.IpGeolocation.Common.Models
{
    /// <summary>
    /// Represents generic API related exceptions when no other <see cref="ApiException"/> is appropriate
    /// </summary>
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
    }

    /// <summary>
    /// Represents an error of unknown cause. Typically used as a bare-bone <see cref="Exception"/> alternative
    /// without the extra info it carries
    /// </summary>
    public class UnknownErrorException : ApiException
    {
        public UnknownErrorException() : base(Exceptions.UnknownError) { }
    }
}
