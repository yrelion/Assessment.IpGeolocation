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

namespace Novibet.Service.IpGeolocation.Controllers
{
    [Route("v1/[controller]")]
    [ApiController, HandleExceptions]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class GeolocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GeolocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get an IP's geolocation information
        /// </summary>
        /// <param name="ip">The IP address to query information for</param>
        /// <returns>An <see cref="IPGeolocation"/> information object</returns>
        [HttpGet("{ip}")]
        [ProducesResponseType(typeof(IPGeolocation), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetails([FromRoute] string ip)
        {
            var query = new GetIpDetailsQuery(ip);
            var result = await _mediator.Send(query);

            if(result == null)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return Ok(result);
        }

        /// <summary>
        /// Get a batch job status
        /// </summary>
        /// <param name="batchId">The batch <see cref="Guid"/> to search</param>
        [HttpGet("batch/{batchId}")]
        [ProducesResponseType(StatusCodes.Status102Processing)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBatchStatus([FromRoute] Guid batchId)
        {
            return Ok();
        }

        /// <summary>
        /// Create a batch job to update IP details
        /// </summary>
        /// <param name="request">The <see cref="IPGeolocationUpdateRequest"/>s</param>
        /// <returns>The batch job <see cref="Guid"/></returns>
        [HttpPost("batch")]
        [ProducesResponseType(typeof(Guid),StatusCodes.Status202Accepted)]
        public async Task<IActionResult> UpdateDetails([FromBody] IEnumerable<IPGeolocationUpdateRequest> request)
        {
            var result = Guid.NewGuid(); //TODO: Implement
            return AcceptedAtAction(nameof(GetBatchStatus), new { batchId = result }, result);
        }
    }
}