﻿using Fluent.Swagger.Validation.Resolvers;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Fluent.Swagger.Validation
{
    public class FluentSwaggerValidationSchemeFilter : ISchemaFilter
    {
        private readonly IValidatorFactory validatorFactory;
        private readonly ILogger<FluentSwaggerValidationSchemeFilter> logger;
        private readonly IEnumerable<IResolver> resolvers;

        public FluentSwaggerValidationSchemeFilter(
            IValidatorFactory validatorFactory, 
            ILogger<FluentSwaggerValidationSchemeFilter> logger, 
            IEnumerable<IResolver> resolvers)
        {
            this.validatorFactory = validatorFactory;
            this.logger = logger;
            this.resolvers = resolvers;
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
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
                if (rule is not PropertyRule propertyRule)
                {
                    logger.LogDebug($"Skipped '{rule}'");
                    continue;
                }

                logger.LogDebug($"Rule '{propertyRule.Expression}'");

                foreach (var propertyValidator in rule.Validators)
                {
                    logger.LogDebug($"Property validator '{propertyValidator}'");

                    foreach (var resolver in resolvers.Where(r => r.MatchFunc(propertyValidator)))
                    {
                        resolver.Resolve(schema, context, propertyRule, propertyValidator, validatorFactory, resolvers);
                    }

                }
            }
        }
    }
}
