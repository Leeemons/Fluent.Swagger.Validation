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
    public class RegularExpressionResolver : IResolver
    {
        public Func<IPropertyValidator, bool> MatchFunc => v => v is RegularExpressionValidator;

        public Task Resolve(OpenApiSchema schema, SchemaFilterContext context, PropertyRule propertyRule, IPropertyValidator propertyValidator, IValidatorFactory validatorFactory, IEnumerable<IResolver> resolvers)
        {
            if (propertyRule.HasConditions() || propertyValidator.HasConditions()) return Task.CompletedTask;

            var schemaProperty = schema.Properties[propertyRule.GetPropertyKey()];
            var regularExpressionValidator = (RegularExpressionValidator)propertyValidator;

            schemaProperty.Pattern = regularExpressionValidator.Expression;

            return Task.CompletedTask;
        }
    }
}