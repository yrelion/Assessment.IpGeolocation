using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Assessment.IpGeolocation.Attributes;
using Assessment.IpGeolocation.Common.Models;
using Assessment.IpGeolocation.Core.Requests.Commands;
using Assessment.IpGeolocation.Core.Requests.Queries;

namespace Assessment.IpGeolocation.Controllers
{
    [Route("v1/[controller]")]
    [ApiController, HandleExceptions, ValidateModelState]
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
        /// Create a batch job to update IP details
        /// </summary>
        /// <param name="request">The <see cref="IPGeolocationUpdateRequest"/>s</param>
        /// <returns>The batch job <see cref="Guid"/></returns>
        [HttpPost("batch")]
        [ProducesResponseType(typeof(Guid),StatusCodes.Status202Accepted)]
        public async Task<IActionResult> UpdateDetails([FromBody] IEnumerable<IPGeolocationUpdateRequest> request)
        {
            var command = new UpdateBatchIPGeolocationCommand(request);
            var result = await _mediator.Send(command);

            return AcceptedAtAction(nameof(GetBatchStatus), new { jobId = result }, result);
        }

        /// <summary>
        /// Get a batch job status
        /// </summary>
        /// <param name="jobId">The batch <see cref="Guid"/> to search</param>
        [HttpGet("batch/{jobId}")]
        [ProducesResponseType(typeof(BackgroundJobStatus), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBatchStatus([FromRoute] Guid jobId)
        {
            var query = new GetIpGeolocationBatchUpdateStatusQuery(jobId);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}