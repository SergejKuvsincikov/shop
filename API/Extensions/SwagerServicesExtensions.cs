using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwagerServicesExtensions
    {
        public static IServiceCollection AddSwagerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop", Version = "v1" });
            });
            return services;
        }
        public static IApplicationBuilder  UseSwagerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop"));
            return app;
        }
    }
}