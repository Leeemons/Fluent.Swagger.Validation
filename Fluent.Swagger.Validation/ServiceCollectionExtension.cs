using Fluent.Swagger.Validation.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fluent.Swagger.Validation
{
    public static class ServiceCollectionExtension
    {
        public static void AddFluentSwaggerValidation(this IServiceCollection services)
        {
            services.Scan(a =>
                a.FromAssembliesOf(typeof(ServiceCollectionExtension))
                .AddClasses(c => c.AssignableTo<IResolver>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
        }
    }
}
