using Fluent.Swagger.Validation.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection;

namespace Fluent.Swagger.Validation
{
    public static class ServiceCollectionExtension
    {
        public static void AddFluentSwaggerValidation(this IServiceCollection services, params Type[] types)
        {
            var typesList = types.ToList();
            typesList.Add(typeof(ServiceCollectionExtension));

            services.Scan(a =>
                a.FromAssembliesOf(typesList)
                .AddClasses(c => c.AssignableTo<IResolver>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
        }
    }
}
