using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Novibet.Service.IpGeolocation.Data.Models;

namespace Novibet.Service.IpGeolocation.Data
{
    public class GeolocationContext : DbContext
    {
        public DbSet<IPGeolocationDto> Geolocations { get; set; }

        public GeolocationContext(DbContextOptions<GeolocationContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
    }
}
