using System;
using System.Collections.Generic;
using System.Text;
using Novibet.Service.IpGeolocation.Common.Interfaces;

namespace Novibet.Service.IpGeolocation.Common.Models
{
    public class ServiceConfiguration : IServiceProxySettings
    {
        /// <summary>
        /// The service identifier
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// The friendly service name
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The base URL of the API
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// The content type header value
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The access key to authenticate with
        /// </summary>
        public string AccessKey { get; set; }
    }
}
