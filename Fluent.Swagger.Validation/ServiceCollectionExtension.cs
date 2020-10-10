using Fluent.Swagger.Validation.Resolvers;
using Microsoft.Extensions.DependencyInjection;

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
