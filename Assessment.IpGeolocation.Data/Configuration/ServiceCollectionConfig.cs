using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assessment.IpGeolocation.Data.Configuration
{
    public static class ServiceCollectionConfig
    {
        public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseContexts(configuration);
        }

        private static void AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GeolocationContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
