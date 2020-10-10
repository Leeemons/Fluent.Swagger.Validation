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
    public class NotNullResolver : IResolver
    {
        public Func<IPropertyValidator, bool> MatchFunc => v => v is NotNullValidator;

        public Task Resolve(
            OpenApiSchema schema,
            SchemaFilterContext context,
            PropertyRule propertyRule,
            IPropertyValidator propertyValidator,
            IValidatorFactory validatorFactory,
            IEnumerable<IResolver> resolvers)
        {
            if (propertyRule.HasConditions() || propertyValidator.HasConditions()) return Task.CompletedTask;

            if (schema.Required == null) schema.Required = new SortedSet<string>();            
            if (!schema.Required.Contains(propertyRule.GetPropertyKey()))
            {
                schema.Required.Add(propertyRule.GetPropertyKey());
                schema.Properties[propertyRule.GetPropertyKey()].Nullable = false;
            }

            return Task.CompletedTask;
        }
    }
}
