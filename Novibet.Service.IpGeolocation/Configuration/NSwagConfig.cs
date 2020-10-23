using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace Novibet.Service.IpGeolocation.Configuration
{
    public static class NSwagConfig
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            void DocumentSettingsDelegate(AspNetCoreOpenApiDocumentGeneratorSettings config)
            {
                config.Title = "Novibet.IpGeolocation";
                config.Version = "v1";
                config.DocumentName = "Novibet.IpGeolocation.Api";

                config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                });

                config.GenerateEnumMappingDescription = true;
                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            }

            //services.AddOpenApiDocument(DocumentSettingsDelegate);  // Produces spec of type: OpenAPI v3
            services.AddSwaggerDocument(DocumentSettingsDelegate);  // Produces spec of type: Swagger v2.0
        }

        public static void ConfigureSwaggerUi(this IApplicationBuilder app)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3(options => {
                options.DefaultModelsExpandDepth = -1;
            });
            app.UseReDoc(options => { options.Path = "/reference"; });
        }
    }
}
