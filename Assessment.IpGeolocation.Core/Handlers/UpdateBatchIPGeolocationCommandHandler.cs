using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Assessment.IpGeolocation.Common.Interfaces;
using Assessment.IpGeolocation.Common.Models;
using Assessment.IpGeolocation.Core.Requests.Commands;
using Assessment.IpGeolocation.Core.Services;
using Assessment.IpGeolocation.Data;
using Assessment.IpGeolocation.Data.Models;

namespace Assessment.IpGeolocation.Core.Handlers
{
    public class UpdateBatchIPGeolocationCommandHandler : IRequestHandler<UpdateBatchIPGeolocationCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly ICacheProvider _cacheProvider;
        private readonly IPGeolocationBatchUpdateService _batchUpdateService;
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateBatchIPGeolocationCommandHandler(IMapper mapper, ICacheProvider cacheProvider, 
            IPGeolocationBatchUpdateService batchUpdateService, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _cacheProvider = cacheProvider;
            _batchUpdateService = batchUpdateService;
            _scopeFactory = scopeFactory;
        }

        public async Task<Guid> Handle(UpdateBatchIPGeolocationCommand command, CancellationToken cancellationToken)
        {
            var generatedId =_batchUpdateService.AddJob(command.Request.ToList(), requests =>
            {
                requests.ToList().ForEach(async x =>
                {
                    _cacheProvider.Remove(x.Ip);
                    await _cacheProvider.GetOrCreateAsync(x.Ip, () => UpdateGeolocation(x));
                });
            });

            return generatedId;
        }

        /// <summary>
        /// Updates the <see cref="IPGeolocation"/> database entry by the given request
        /// </summary>
        /// <param name="request">The request to update the <see cref="IPGeolocation"/> entry with</param>
        /// <returns>The updated <see cref="IPGeolocation"/></returns>
        private async Task<IPGeolocation> UpdateGeolocation(IPGeolocationUpdateRequest request)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                /*
                 * Workaround for hosted service not aware of injected scoped services due to
                 * their disposal when the request lifecycle ends
                 */
                var context = scope.ServiceProvider.GetRequiredService<GeolocationContext>();

                var entity = await context.Geolocations.FindAsync(request.Ip);

                if (entity == null)
                    return null;

                entity = Decorate(entity, request);

                context.Geolocations.Update(entity);
                await context.SaveChangesAsync();

                var updatedGeolocation = await context.Geolocations.FindAsync(entity.Id);
                var result = _mapper.Map<IPGeolocation>(updatedGeolocation);

                return result;
            }
        }

        /// <summary>
        /// Decorates the DTO by mapping relevant properties from the <see cref="IPGeolocationUpdateRequest"/>
        /// </summary>
        /// <param name="dto">The DTO to decorate</param>
        /// <param name="request">The <see cref="IPGeolocationUpdateRequest"/> to decorate the DTO with</param>
        private IPGeolocationDto Decorate(IPGeolocationDto dto, IPGeolocationUpdateRequest request)
        {
            dto.City = request.City;
            dto.Country = request.Country;
            dto.Continent = request.Continent;
            dto.Latitude = request.Latitude;
            dto.Longitude = request.Longitude;

            return dto;
        }
    }
}
