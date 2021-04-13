using Fluent.Swagger.Validation.Resolvers;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent.Swagger.Validation
{
    public class FluentSwaggerValidationSchemeFilter : ISchemaFilter
    {
        private readonly ILogger<FluentSwaggerValidationSchemeFilter> logger;
        private readonly IEnumerable<IResolver> resolvers;
        private readonly IServiceProvider serviceProvider;

        public FluentSwaggerValidationSchemeFilter(
            IServiceProvider serviceProvider,
            ILogger<FluentSwaggerValidationSchemeFilter> logger,
            IEnumerable<IResolver> resolvers)
        {
            this.logger = logger;
            this.resolvers = resolvers;
            this.serviceProvider = serviceProvider;
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            using var scope = serviceProvider.CreateScope();
            var validatorFactory = scope.ServiceProvider.GetRequiredService<IValidatorFactory>();

            var type = context.Type;

            if (validatorFactory.GetValidator(type) is not IEnumerable<IValidationRule> validators)
            {
                logger.LogDebug($"Skipped '{type.Name}'");
                return;
            }

            if (schema.Properties == null)
            {
                logger.LogDebug($"Skipped '{type.Name}'");
                return;
            }

            foreach (var rule in validators)
            {
                if (rule is not IValidationRule validationRule)
                {
                    logger.LogDebug($"Skipped '{rule}'");
                    continue;
                }

                logger.LogDebug($"Rule '{validationRule.Expression}'");

                foreach (var ruleComponent in rule.Components)
                {
                    logger.LogDebug($"Rule component '{ruleComponent}'");

                    foreach (var resolver in resolvers.Where(r => r.MatchFunc(ruleComponent)))
                    {
                        resolver.Resolve(schema, context, validationRule, ruleComponent, validatorFactory, resolvers);
                    }
                }
            }
        }
    }
}