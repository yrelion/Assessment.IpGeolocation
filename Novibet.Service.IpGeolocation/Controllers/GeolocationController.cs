using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Novibet.Service.IpGeolocation.Common.Interfaces;
using Novibet.Service.IpGeolocation.Proxies.Interfaces;

namespace Novibet.Service.IpGeolocation.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class GeolocationController : ControllerBase
    {
        private readonly IIPInfoProvider _ipInfoProvider;

        public GeolocationController(IIPInfoProvider ipInfoProvider)
        {
            _ipInfoProvider = ipInfoProvider;
        }

        [HttpGet("{ip}")]
        [ProducesResponseType(typeof(IPDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetails([FromRoute] string ip)
        {
            var result = _ipInfoProvider.GetDetails(ip);

            return Ok(result);
        }
    }
}