using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Novibet.Service.IpGeolocation.Attributes;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Core.Requests;
using Novibet.Service.IpGeolocation.Proxies.Interfaces;

namespace Novibet.Service.IpGeolocation.Controllers
{
    [Route("v1/[controller]")]
    [ApiController, HandleExceptions]
    public class GeolocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GeolocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{ip}")]
        [ProducesResponseType(typeof(IPLookupDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetails([FromRoute] string ip)
        {
            var query = new GetIpDetailsQuery(ip);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}