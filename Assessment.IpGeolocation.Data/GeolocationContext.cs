using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Assessment.IpGeolocation.Data.Models;

namespace Assessment.IpGeolocation.Data
{
    public class GeolocationContext : DbContext
    {
        public DbSet<IPGeolocationDto> Geolocations { get; set; }

        public GeolocationContext(DbContextOptions<GeolocationContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
    }
}
