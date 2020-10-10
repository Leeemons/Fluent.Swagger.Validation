using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fluent.Swagger.Validation.Resolvers
{
    public class NotEmptyResolver : IResolver
    {
        public Func<IPropertyValidator, bool> MatchFunc => v => v is LessThanOrEqualValidator;

        public Task Resolve(OpenApiSchema schema, SchemaFilterContext context, PropertyRule propertyRule, IPropertyValidator propertyValidator, IValidatorFactory validatorFactory, IEnumerable<IResolver> resolvers)
        {
            if (propertyRule.HasConditions() || propertyValidator.HasConditions()) return Task.CompletedTask;
            
            var schemaProperty = schema.Properties[propertyRule.GetPropertyKey()];
            schemaProperty.MinLength = 1;
            schemaProperty.Nullable = false;
            return Task.CompletedTask;

        }
    }
}
