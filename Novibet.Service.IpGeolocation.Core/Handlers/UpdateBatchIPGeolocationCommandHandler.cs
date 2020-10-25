using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Novibet.Service.IpGeolocation.Common.Interfaces;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Core.Models;
using Novibet.Service.IpGeolocation.Core.Requests.Commands;
using Novibet.Service.IpGeolocation.Core.Services;
using Novibet.Service.IpGeolocation.Data;
using Novibet.Service.IpGeolocation.Data.Models;

namespace Novibet.Service.IpGeolocation.Core.Handlers
{
    class UpdateBatchIPGeolocationCommandHandler : IRequestHandler<UpdateBatchIPGeolocationCommand, Guid>
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
                    await _cacheProvider.GetOrCreateAsync(x.Ip, UpdateGeolocation(x));
                });
            });

            return generatedId;
        }

        private async Task<IPGeolocation> UpdateGeolocation(IPGeolocationUpdateRequest updateRequest)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GeolocationContext>();

                var newGeolocation = _mapper.Map<IPGeolocationDto>(updateRequest);
                var entity = await context.Geolocations.FindAsync(updateRequest.Ip);

                if (entity == null)
                    return null;

                entity.City = updateRequest.City;
                entity.Country = updateRequest.Country;
                entity.Continent = updateRequest.Continent;
                entity.Latitude = updateRequest.Latitude;
                entity.Longitude = updateRequest.Longitude;

                context.Geolocations.Update(entity);

                //context.Entry(entity).CurrentValues.SetValues(newGeolocation);
                await context.SaveChangesAsync();

                var result = _mapper.Map<IPGeolocation>(newGeolocation);
                return result;
            }
        }
    }
}
