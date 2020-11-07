using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.IpGeolocation.Common.Interfaces
{
    public interface IHostedWorkerSettings
    {
        /// <summary>
        /// Interval in milliseconds
        /// </summary>
        int Interval { get; set; }
    }
}
