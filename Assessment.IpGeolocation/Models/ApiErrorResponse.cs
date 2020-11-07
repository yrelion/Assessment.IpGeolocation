using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.IpGeolocation.Models
{
    public class ApiErrorResponse
    {
        public IEnumerable<string> Messages { get; set; }
        public string TraceId { get; set; }
    }
}
