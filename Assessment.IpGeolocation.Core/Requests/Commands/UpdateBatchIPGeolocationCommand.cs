using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assessment.IpGeolocation.Common.Interfaces;
using MediatR;
using Assessment.IpGeolocation.Common.Models;
using Assessment.IpGeolocation.Data;
using Assessment.IpGeolocation.Data.Models;
using Assessment.IpGeolocation.Proxies.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assessment.IpGeolocation.Core.Requests.Commands
{
    public class UpdateBatchIPGeolocationCommand : IRequest<Guid>
    {
        public readonly IEnumerable<IPGeolocationUpdateRequest> Request;

        public UpdateBatchIPGeolocationCommand(IEnumerable<IPGeolocationUpdateRequest> request)
        {
            Request = request;
        }

        public class GetIpDetailsQuery : IRequest<IPGeolocation>
        {
            public readonly string IP;

            public GetIpDetailsQuery(string ip)
            {
                IP = ip;
            }

            public class GetIpDetailsHandler : IRequestHandler<Queries.GetIpDetailsQuery, IPGeolocation>
            {
                private readonly IMapper _mapper;
                private readonly IIPInfoProvider _ipInfoProvider;
                private readonly GeolocationContext _geolocationContext;
                private readonly ICacheProvider _cacheProvider;

                public GetIpDetailsHandler(IMapper mapper, IIPInfoProvider ipInfoProvider, GeolocationContext geolocationContext,
                    ICacheProvider cacheProvider)
                {
                    _mapper = mapper;
                    _ipInfoProvider = ipInfoProvider;
                    _geolocationContext = geolocationContext;
                    _cacheProvider = cacheProvider;
                }

                public async Task<IPGeolocation> Handle(Queries.GetIpDetailsQuery request, CancellationToken cancellationToken)
                {
                    var result = await _cacheProvider.GetOrCreateAsync(request.IP, () => RetrieveDetails(request));
                    return result;
                }

                /// <summary>
                /// Queries geolocation information from the database, or when not available it
                /// requests the relevant data from the <see cref="IIPInfoProvider"/> and saves it
                /// </summary>
                /// <param name="request">The <see cref="Queries.GetIpDetailsQuery"/></param>
                /// <returns>The <see cref="IPGeolocation"/> information</returns>
                protected async Task<IPGeolocation> RetrieveDetails(Queries.GetIpDetailsQuery request)
                {
                    // Retrieve
                    var dbGeolocation = await _geolocationContext.Geolocations.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == request.IP);

                    if (dbGeolocation != null)
                        return _mapper.Map<IPGeolocation>(dbGeolocation);

                    // Request
                    var geolocation = await GetGeolocation(request.IP);

                    if (geolocation == null)
                        return null;

                    // Store
                    var geolocationSaved = await SaveGeolocation(geolocation, request.IP);

                    if (!geolocationSaved)
                        return null;

                    return geolocation;
                }

                /// <summary>
                /// Requests geolocation information for the specified IP address
                /// </summary>
                /// <param name="ip">The IP address to request information for</param>
                /// <returns>The <see cref="IPGeolocation"/> information</returns>
                protected async Task<IPGeolocation> GetGeolocation(string ip)
                {
                    var response = await Task.Run(() => _ipInfoProvider.GetDetails(ip)); // TODO: Review interface constraint
                    return _mapper.Map<IPGeolocation>(response);
                }

                /// <summary>
                /// Stores the geolocation information to the <see cref="DbContext"/>
                /// </summary>
                /// <param name="geolocation">The <see cref="IPGeolocation"/> object to store</param>
                /// <param name="id">The IP address as the database entry identifier</param>
                /// <returns>The database operation fulfillment</returns>
                protected async Task<bool> SaveGeolocation(IPGeolocation geolocation, string id)
                {
                    var geolocationDto = _mapper.Map<IPGeolocationDto>(geolocation);
                    geolocationDto.Id = id;

                    _geolocationContext.Geolocations.Add(geolocationDto);
                    var rowsAffected = await _geolocationContext.SaveChangesAsync();

                    return rowsAffected == 1;
                }
            }
        }
    }
}
